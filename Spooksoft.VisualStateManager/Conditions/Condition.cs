using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public static class Condition
    {
        public static AllCondition<T> All<T>(ObservableCollection<T> collection, Func<T, BaseCondition> conditionExtractor, bool valueIfEmpty = false)
            where T : class
            => new AllCondition<T>(collection, conditionExtractor, valueIfEmpty);

        public static AnyCondition<T> Any<T>(ObservableCollection<T> collection, Func<T, BaseCondition> conditionExtractor, bool valueIfEmpty = false)
            where T : class
            => new AnyCondition<T>(collection, conditionExtractor, valueIfEmpty);

        public static ChainedLambdaCondition<TSource> ChainedLambda<TSource>(TSource source,
            Expression<Func<TSource, bool>> expression,
            bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            => new ChainedLambdaCondition<TSource>(source, expression, defaultValue);

        public static ChainedLambdaCondition<TSource, T1> ChainedLambda<TSource, T1>(TSource source,
            Expression<Func<TSource, T1>> expression1,
            Expression<Func<T1, bool>> expression2,
            bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            where T1 : class, INotifyPropertyChanged
            => new ChainedLambdaCondition<TSource, T1>(source, expression1, expression2, defaultValue);

        public static ChainedLambdaCondition<TSource, T1, T2> ChainedLambda<TSource, T1, T2>(TSource source,
            Expression<Func<TSource, T1>> expression1,
            Expression<Func<T1, T2>> expression2,
            Expression<Func<T2, bool>> expression3,
            bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            where T1 : class, INotifyPropertyChanged
            where T2 : class, INotifyPropertyChanged
            => new ChainedLambdaCondition<TSource, T1, T2>(source, expression1, expression2, expression3, defaultValue);

        public static ChainedLambdaCondition<TSource, T1, T2, T3> ChainedLambda<TSource, T1, T2, T3>(TSource source,
            Expression<Func<TSource, T1>> expression1,
            Expression<Func<T1, T2>> expression2,
            Expression<Func<T2, T3>> expression3,
            Expression<Func<T3, bool>> expression4,
            bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            where T1 : class, INotifyPropertyChanged
            where T2 : class, INotifyPropertyChanged
            where T3 : class, INotifyPropertyChanged
            => new ChainedLambdaCondition<TSource, T1, T2, T3>(source, expression1, expression2, expression3, expression4, defaultValue);

        public static CompositeCondition Composite(CompositeCondition.CompositionKind kind, params BaseCondition[] compositeConditions)
            => new CompositeCondition(kind, compositeConditions);

        public static CompositeCondition Composite(CompositeCondition.CompositionKind kind)
            => new CompositeCondition(kind);

        public static FalseCondition False()
            => new FalseCondition();

        public static LambdaCondition<TSource> Lambda<TSource>(TSource source, Expression<Func<TSource, bool>> lambda, bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            => new LambdaCondition<TSource>(source, lambda, defaultValue);

        public static NegateCondition Negate(BaseCondition condition) 
            => new NegateCondition(condition);

        public static PropertyWatchCondition<TSource> PropertyWatch<TSource>(TSource source, Expression<Func<TSource, bool>> expression, bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            => new PropertyWatchCondition<TSource>(source, expression, defaultValue);

        public static PropertyWatchCondition<TSource> PropertyWatch<TSource>(Expression<Func<TSource, bool>> expression, bool defaultValue)
            where TSource : class, INotifyPropertyChanged
            => new PropertyWatchCondition<TSource>(expression, defaultValue);

        public static SimpleCondition Simple(bool newValue = false) 
            => new SimpleCondition(newValue);

        public static SwitchCondition<T> Switch<T>()
            where T : struct
            => new SwitchCondition<T>();

        public static SwitchCondition<T> Switch<T>(params T[] newValues)
            where T : struct 
            => new SwitchCondition<T>(newValues);

        public static TrueCondition True() 
            => new TrueCondition();
    }
}
