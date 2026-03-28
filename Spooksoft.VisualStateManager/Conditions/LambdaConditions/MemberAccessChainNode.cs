using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Spooksoft.VisualStateManager.Conditions
{
    /// <summary>
    /// Represents a single node in a chain of property accesses extracted from a lambda
    /// expression (e.g. one segment of a.B.C). Monitors property changes on its source
    /// object and collection changes on its property value, propagating updates down the chain.
    /// </summary>
    internal class MemberAccessChainNode
    {
        private readonly MemberAccessChainNode next;
        private readonly NotificationRegistry notificationRegistry;
        private INotifyPropertyChanged source;
        private INotifyCollectionChanged trackedCollection;
        private MemberExpression expression;

        /// <summary>
        /// Reads the current property value, updates collection tracking,
        /// and propagates the value as the source of the next node in the chain.
        /// </summary>
        private void UpdateNext()
        {
            // Read the property value (even for leaf nodes, to track collection changes)
            object value = source != null
                ? (expression.Member as PropertyInfo).GetValue(source)
                : null;

            UpdateCollectionTracking(value);

            if (next == null)
                return;

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

        /// <summary>
        /// Registers or unregisters collection change tracking when the property value
        /// changes to a different INotifyCollectionChanged instance (or to/from null).
        /// </summary>
        private void UpdateCollectionTracking(object propertyValue)
        {
            var newCollection = propertyValue as INotifyCollectionChanged;

            if (trackedCollection == newCollection)
                return;

            if (trackedCollection != null)
                notificationRegistry.UnregisterCollection(this, trackedCollection);

            trackedCollection = newCollection;

            if (trackedCollection != null)
                notificationRegistry.RegisterCollection(this, trackedCollection);
        }

        /// <summary>
        /// Creates a new node for the given member-access expression, linked
        /// to an optional next node in the chain.
        /// </summary>
        internal MemberAccessChainNode(MemberAccessChainNode next, NotificationRegistry notificationRegistry, MemberExpression expression)
        {
            if (expression.Member.MemberType != MemberTypes.Property ||
                !(expression.Member is PropertyInfo))
                throw new ArgumentException("Currenlty only property member accesses are implemented!");

            this.next = next;
            this.notificationRegistry = notificationRegistry;
            this.expression = expression;
        }

        /// <summary>
        /// Called when the source object for this node changes. Unregisters from the
        /// old source, registers on the new one, and propagates updates down the chain.
        /// </summary>
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

        /// <summary>
        /// Called when the monitored property on the current source changes.
        /// Re-reads the property value and propagates updates down the chain.
        /// </summary>
        internal void NotifyPropertyChanged()
        {
            UpdateNext();
        }

        /// <summary>
        /// Checks whether this node and all subsequent nodes in the chain
        /// have a valid (non-null) source.
        /// </summary>
        internal bool SourceInChainIsValid()
        {
            return source != null && (next?.SourceInChainIsValid() ?? true);
        }
    }
}
