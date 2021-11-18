using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class LambdaCondition<TSource> : BaseCondition
        where TSource : class, INotifyPropertyChanged
    {
        // Private types ------------------------------------------------------

        private class NotificationRegistry
        {
            private class NotificationEntry
            {
                public NotificationEntry(MemberAccessChainNode node, string propertyName)
                {
                    Node = node;
                    PropertyName = propertyName;
                }

                public MemberAccessChainNode Node { get; }
                public string PropertyName { get; }
            }

            private Dictionary<INotifyPropertyChanged, List<NotificationEntry>> notificationRegistrations = new Dictionary<INotifyPropertyChanged, List<NotificationEntry>>();

            private void HandleTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (notificationRegistrations.TryGetValue(sender as INotifyPropertyChanged, out List<NotificationEntry> registrations))
                {
                    bool updated = false;

                    foreach (var registration in registrations.Where(r => r.PropertyName == e.PropertyName))
                    {
                        registration.Node.NotifyPropertyChanged();
                        updated = true;
                    }

                    if (updated)
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public void Register(MemberAccessChainNode node, string property, INotifyPropertyChanged target)
            {
                if (notificationRegistrations.TryGetValue(target, out List<NotificationEntry> registrations))
                {
                    registrations = notificationRegistrations[target];
                }
                else
                {
                    registrations = new List<NotificationEntry>();
                    notificationRegistrations.Add(target, registrations);
                    target.PropertyChanged += HandleTargetPropertyChanged;
                }

                registrations.Add(new NotificationEntry(node, property));
            }

            public void Unregister(MemberAccessChainNode node, string property, INotifyPropertyChanged target)
            {
                if (notificationRegistrations.ContainsKey(target))
                {
                    var registrations = notificationRegistrations[target];
                    var registration = registrations.FirstOrDefault(r => r.Node == node && r.PropertyName == property);
                    if (registration != null)
                        registrations.Remove(registration);

                    if (registrations.Count == 0)
                    {
                        target.PropertyChanged -= HandleTargetPropertyChanged;
                        notificationRegistrations.Remove(target);
                    }
                }
            }

            public event EventHandler ValueChanged;
        }

        private class MemberAccessChainNode
        {
            private readonly MemberAccessChainNode next;
            private readonly NotificationRegistry notificationRegistry;
            private INotifyPropertyChanged source;
            private MemberExpression expression;

            private void UpdateNext()
            {
                if (next == null)
                    return;

                if (source != null)
                {
                    object value = (expression.Member as PropertyInfo).GetValue(source);

                    if (value != null)
                    {
                        if (!(value is INotifyPropertyChanged notifying))
                            throw new InvalidOperationException($"For LambdaCondition to work properly, all intermediate classes in the member access chain must implement INotifyPropertyChanged. {value.GetType().Name} being value of {source.GetType().Name}.{expression.Member.Name} doesn't match this requirement.");

                        next.NotifySourceChanged(notifying);
                    }
                    else
                    {
                        next.NotifySourceChanged(null);
                    }
                }
                else
                {
                    next.NotifySourceChanged(null);
                }
            }

            internal MemberAccessChainNode(MemberAccessChainNode next, NotificationRegistry notificationRegistry, MemberExpression expression)
            {
                if (expression.Member.MemberType != MemberTypes.Property ||
                    !(expression.Member is PropertyInfo))
                    throw new ArgumentException("Currenlty only property member accesses are implemented!");

                this.next = next;
                this.notificationRegistry = notificationRegistry;
                this.expression = expression;
            }

            internal void NotifySourceChanged(INotifyPropertyChanged newSource)
            {
                if (source == newSource)
                    return;

                // Unregister old source
                if (source != null)
                    notificationRegistry.Unregister(this, expression.Member.Name, source);

                source = newSource;

                if (newSource != null)
                {
                    notificationRegistry.Register(this, expression.Member.Name, source);
                }

                UpdateNext();
            }

            internal void NotifyPropertyChanged()
            {
                UpdateNext();
            }

            internal bool SourceInChainIsValid()
            {
                return source != null && (next?.SourceInChainIsValid() ?? true);
            }
        }

        private class Visitor : ExpressionVisitor
        {
            private readonly List<Expression> expressions = new List<Expression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                expressions.Add(node);
                return base.VisitParameter(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                expressions.Add(node);
                return base.VisitMember(node);
            }

            public IReadOnlyList<Expression> Expressions => expressions;
        }

        // Private fields -----------------------------------------------------

        private readonly List<MemberAccessChainNode> chainNodes;
        private readonly NotificationRegistry notificationRegistry;
        private readonly Func<TSource, bool> func;
        private readonly TSource source;

        private bool defaultValue;
        private bool cachedValue;

        // Private methods ----------------------------------------------------

        private void ProcessLambda(Expression<Func<TSource, bool>> lambda)
        {
            var visitor = new Visitor();
            visitor.Visit(lambda);

            List<List<MemberExpression>> linkedExpressions = new List<List<MemberExpression>>();

            // Chain member access expressions originating at parameter expression

            var allExpressions = new List<Expression>(visitor.Expressions);

            var root = allExpressions.OfType<ParameterExpression>().FirstOrDefault();
            while (root != null)
            {
                allExpressions.Remove(root);

                List<MemberExpression> expressions = new List<MemberExpression>();
                Expression previous = root;
                var next = allExpressions.OfType<MemberExpression>().FirstOrDefault(e => e.Expression == previous);
                while (next != null)
                {
                    allExpressions.Remove(next);

                    expressions.Add(next);
                    previous = next;
                    next = allExpressions.OfType<MemberExpression>().FirstOrDefault(e => e.Expression == previous);
                }

                if (expressions.Count > 0)
                    linkedExpressions.Add(expressions);

                root = allExpressions.OfType<ParameterExpression>().FirstOrDefault();
            }
            
            foreach (var expressions in linkedExpressions)
            {
                MemberAccessChainNode next = null;
                MemberAccessChainNode current = null;

                for (int i = expressions.Count - 1; i >= 0; i--)
                {
                    current = new MemberAccessChainNode(next, notificationRegistry, expressions[i]);
                    next = current;
                }

                if (current != null)
                    chainNodes.Add(current);
            }
        }

        private void HandleIntermediateValueChanged(object sender, EventArgs e)
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            bool newValue;

            // Check if any of intermediate sources is not null
            if (chainNodes.All(n => n.SourceInChainIsValid()))
            {
                try
                {
                    newValue = func(source);
                }
                catch (NullReferenceException)
                {
                    newValue = defaultValue;
                }                
            }
            else
            {
                newValue = defaultValue;
            }

            if (cachedValue != newValue)
            {
                cachedValue = newValue;
                OnValueChanged(cachedValue);
            }
        }

        // Public methods -----------------------------------------------------

        public LambdaCondition(TSource source, Expression<Func<TSource, bool>> lambda, bool defaultValue)
        {
            this.defaultValue = defaultValue;
            this.cachedValue = defaultValue;
            this.source = source;

            chainNodes = new List<MemberAccessChainNode>();
            notificationRegistry = new NotificationRegistry();
            notificationRegistry.ValueChanged += HandleIntermediateValueChanged;

            ProcessLambda(lambda);

            func = lambda.Compile();

            foreach (var chainNode in chainNodes)
                chainNode.NotifySourceChanged(source);

            UpdateValue();
        }

        public override bool GetValue()
        {
            return cachedValue;
        }
    }
}
