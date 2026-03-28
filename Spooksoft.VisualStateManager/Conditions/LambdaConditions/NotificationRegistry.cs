using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Spooksoft.VisualStateManager.Conditions
{
    /// <summary>
    /// Manages subscriptions to PropertyChanged and CollectionChanged events on behalf
    /// of MemberAccessChainNode instances. Raises the ValueChanged event whenever
    /// a monitored property or collection changes.
    /// </summary>
    internal class NotificationRegistry
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
        private Dictionary<INotifyCollectionChanged, List<MemberAccessChainNode>> collectionRegistrations = new Dictionary<INotifyCollectionChanged, List<MemberAccessChainNode>>();

        /// <summary>
        /// Handles PropertyChanged on a monitored source. Notifies matching
        /// chain nodes and raises ValueChanged if any registrations matched.
        /// </summary>
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

        /// <summary>
        /// Handles CollectionChanged on a monitored collection.
        /// Raises ValueChanged so the condition is re-evaluated.
        /// </summary>
        private void HandleTargetCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is INotifyCollectionChanged collection && collectionRegistrations.ContainsKey(collection))
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Registers a chain node to be notified when the specified property
        /// changes on the given INotifyPropertyChanged target.
        /// </summary>
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

        /// <summary>
        /// Removes a previously registered property-change subscription.
        /// Unsubscribes from the target's PropertyChanged when no registrations remain.
        /// </summary>
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

        /// <summary>
        /// Registers a chain node to be notified when the given
        /// INotifyCollectionChanged target raises CollectionChanged.
        /// </summary>
        public void RegisterCollection(MemberAccessChainNode node, INotifyCollectionChanged target)
        {
            if (collectionRegistrations.TryGetValue(target, out List<MemberAccessChainNode> nodes))
            {
                nodes = collectionRegistrations[target];
            }
            else
            {
                nodes = new List<MemberAccessChainNode>();
                collectionRegistrations.Add(target, nodes);
                target.CollectionChanged += HandleTargetCollectionChanged;
            }

            nodes.Add(node);
        }

        /// <summary>
        /// Removes a previously registered collection-change subscription.
        /// Unsubscribes from the target's CollectionChanged when no registrations remain.
        /// </summary>
        public void UnregisterCollection(MemberAccessChainNode node, INotifyCollectionChanged target)
        {
            if (collectionRegistrations.ContainsKey(target))
            {
                var nodes = collectionRegistrations[target];
                nodes.Remove(node);

                if (nodes.Count == 0)
                {
                    target.CollectionChanged -= HandleTargetCollectionChanged;
                    collectionRegistrations.Remove(target);
                }
            }
        }

        public event EventHandler ValueChanged;
    }
}
