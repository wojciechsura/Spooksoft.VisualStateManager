using Spooksoft.VisualStateManager.Conditions.LambdaConditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    /// <summary>
    /// A chained lambda condition with no intermediate steps. Evaluates a boolean
    /// expression directly against the source object.
    /// </summary>
    public class ChainedLambdaCondition<TInput> : BaseChainedLambdaCondition<TInput>
        where TInput : class, INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a condition that evaluates the expression against the given source.
        /// </summary>
        [Obsolete("Please use Condition.ChainedLambda instead.")]
        public ChainedLambdaCondition(TInput source, 
            Expression<Func<TInput, bool>> expression,
            bool defaultValue)
            : base(expression, defaultValue)
        {
            ((IBaseChainedLambdaStep<TInput>)this).UpdateSource(source, true);
        }
    }

    /// <summary>
    /// A chained lambda condition with one intermediate step (TInput → T1 → bool).
    /// </summary>
    public class ChainedLambdaCondition<TInput, T1> : BaseChainedLambdaCondition<T1>
        where TInput : class, INotifyPropertyChanged
        where T1 : class, INotifyPropertyChanged
    {
        private readonly IBaseChainedLambdaStep<TInput> firstStep;

        /// <summary>
        /// Creates a condition that chains TInput → T1, then evaluates a boolean expression against T1.
        /// </summary>
        [Obsolete("Please use Condition.ChainedLambda instead.")]
        public ChainedLambdaCondition(TInput source,
            Expression<Func<TInput, T1>> expression1,
            Expression<Func<T1, bool>> expression2,
            bool defaultValue)
            : base(expression2, defaultValue)
        {
            firstStep = new BaseChainedLambdaCondition<T1>.ChainStep<TInput, T1>(expression1, this);
            firstStep.UpdateSource(source, true);
        }
    }

    /// <summary>
    /// A chained lambda condition with two intermediate steps (TInput → T1 → T2 → bool).
    /// </summary>
    public class ChainedLambdaCondition<TInput, T1, T2> : BaseChainedLambdaCondition<T2>
        where TInput : class, INotifyPropertyChanged
        where T1 : class, INotifyPropertyChanged
        where T2 : class, INotifyPropertyChanged
    {
        private readonly IBaseChainedLambdaStep<TInput> firstStep;

        /// <summary>
        /// Creates a condition that chains TInput → T1 → T2, then evaluates a boolean expression against T2.
        /// </summary>
        [Obsolete("Please use Condition.ChainedLambda instead.")]
        public ChainedLambdaCondition(TInput source,
            Expression<Func<TInput, T1>> expression1,
            Expression<Func<T1, T2>> expression2,
            Expression<Func<T2, bool>> expression3,
            bool defaultValue)
            : base(expression3, defaultValue)
        {
            var secondStep = new ChainStep<T1, T2>(expression2, this);
            firstStep = new ChainStep<TInput, T1>(expression1, secondStep);
            firstStep.UpdateSource(source, true);
        }
    }

    /// <summary>
    /// A chained lambda condition with three intermediate steps (TInput → T1 → T2 → T3 → bool).
    /// </summary>
    public class ChainedLambdaCondition<TInput, T1, T2, T3> : BaseChainedLambdaCondition<T3>
        where TInput : class, INotifyPropertyChanged
        where T1 : class, INotifyPropertyChanged
        where T2 : class, INotifyPropertyChanged
        where T3 : class, INotifyPropertyChanged
    {
        private readonly IBaseChainedLambdaStep<TInput> firstStep;

        /// <summary>
        /// Creates a condition that chains TInput → T1 → T2 → T3, then evaluates
        /// a boolean expression against T3.
        /// </summary>
        [Obsolete("Please use Condition.ChainedLambda instead.")]
        public ChainedLambdaCondition(TInput source,
            Expression<Func<TInput, T1>> expression1,
            Expression<Func<T1, T2>> expression2,
            Expression<Func<T2, T3>> expression3,
            Expression<Func<T3, bool>> expression4,
            bool defaultValue)
            : base(expression4, defaultValue)
        {
            var thirdStep = new ChainStep<T2, T3>(expression3, this);
            var secondStep = new ChainStep<T1, T2>(expression2, thirdStep);
            firstStep = new ChainStep<TInput, T1>(expression1, secondStep);
            firstStep.UpdateSource(source, true);
        }
    }
}
