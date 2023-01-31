using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public abstract class BaseCollectionCondition<T> : BaseCondition
        where T : class
    {
        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems)
                            PartialItemAdded((T)item);

                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        // Nothing to do
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems)
                            PartialItemRemoved((T)oldItem);

                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        foreach (var oldItem in e.OldItems)
                            PartialItemRemoved((T)oldItem);

                        foreach (var newItem in e.NewItems)
                            PartialItemAdded((T)newItem);

                        break;
                    }
                case NotifyCollectionChangedAction.Reset:
                    {
                        PartialAllItemsRemoved();

                        foreach (var item in collection)
                            PartialItemAdded(item);

                        break;
                    }
            }

            AfterCollectionChanged();
        }

        protected readonly ObservableCollection<T> collection;

        protected abstract void PartialAllItemsRemoved();

        protected abstract void PartialItemAdded(T item);

        protected abstract void PartialItemRemoved(T item);

        protected abstract void AfterCollectionChanged();

        protected BaseCollectionCondition(ObservableCollection<T> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            collection.CollectionChanged += HandleCollectionChanged;
        }
    }
}
