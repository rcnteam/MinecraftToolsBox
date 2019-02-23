using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace MinecraftToolsBoxSDK
{
    public class TreeDataGridElement : ContentElement
    {
        private const string NullItemError = "The item added to the collection cannot be null.";

        public static readonly RoutedEvent ExpandingEvent;
        public static readonly RoutedEvent ExpandedEvent;
        public static readonly RoutedEvent CollapsingEvent;
        public static readonly RoutedEvent CollapsedEvent;
        public static readonly DependencyProperty HasChildrenProperty;
        public static readonly DependencyProperty IsExpandedProperty;
        public static readonly DependencyProperty LevelProperty;

        public TreeDataGridElement Parent { get; private set; }
        public TreeDataGridModel Model { get; private set; }
        public ObservableCollection<TreeDataGridElement> Children { get; private set; }

        static TreeDataGridElement()
        {
            // Register dependency properties
            HasChildrenProperty = RegisterHasChildrenProperty();
            IsExpandedProperty = RegisterIsExpandedProperty();
            LevelProperty = RegisterLevelProperty();

            // Register routed events
            ExpandingEvent = RegisterExpandingEvent();
            ExpandedEvent = RegisterExpandedEvent();
            CollapsingEvent = RegisterCollapsingEvent();
            CollapsedEvent = RegisterCollapsedEvent();
        }

        public TreeDataGridElement()
        {
            // Initialize the element
            Children = new ObservableCollection<TreeDataGridElement>();

            // Attach events
            Children.CollectionChanged += OnChildrenChanged;
        }

        internal void SetModel(TreeDataGridModel model, TreeDataGridElement parent = null)
        {
            // Set the element information
            Model = model;
            Parent = parent;
            Level = ((parent != null) ? parent.Level + 1 : 0);

            // Iterate through all child elements
            foreach (TreeDataGridElement child in Children)
            {
                // Set the model for the child
                child.SetModel(model, this);
            }
        }

        protected virtual void OnExpanding() { }
        protected virtual void OnExpanded() { }
        protected virtual void OnCollapsing() { }
        protected virtual void OnCollapsed() { }

        private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // Process the event
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    // Process added child
                    OnChildAdded(args.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Replace:

                    // Process replaced child
                    OnChildReplaced((TreeDataGridElement)args.OldItems[0], args.NewItems[0], args.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:

                    // Process removed child
                    OnChildRemoved((TreeDataGridElement)args.OldItems[0]);
                    break;

                case NotifyCollectionChangedAction.Reset:

                    // Process cleared children
                    OnChildrenCleared(args.OldItems);
                    break;
            }
        }

        private void OnChildAdded(object item)
        {
            // Verify the new child
            TreeDataGridElement child = VerifyItem(item);

            // Set the model for the child
            child.SetModel(Model, this);

            // Notify the model that a child was added to the item
            Model?.OnChildAdded(child);

            SetValue(HasChildrenProperty, true);
        }

        private void OnChildReplaced(TreeDataGridElement oldChild, object item, int index)
        {
            // Verify the new child
            TreeDataGridElement child = VerifyItem(item);

            // Clear the model for the old child
            oldChild.SetModel(null);

            // Notify the model that a child was replaced
            Model?.OnChildReplaced(oldChild, child, index);
        }

        private void OnChildRemoved(TreeDataGridElement child)
        {
            // Clear the model for the child
            child.SetModel(null);

            // Notify the model that a child was removed from the item
            Model?.OnChildRemoved(child);
        }

        private void OnChildrenCleared(IList children)
        {
            // Iterate through all of the children
            foreach (TreeDataGridElement child in children)
            {
                // Clear the model for the child
                child.SetModel(null);
            }

            // Notify the model that all of the children were removed from the item
            Model?.OnChildrenRemoved(this, children);
        }

        internal static TreeDataGridElement VerifyItem(object item)
        {
            // Is the item valid?
            if (item == null)
            {
                // The item is not valid
                throw new ArgumentNullException(nameof(item), NullItemError);
            }

            // Return the element
            return (TreeDataGridElement)item;
        }

        private static void OnIsExpandedChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            // Get the tree item
            TreeDataGridElement item = (TreeDataGridElement)element;

            // Is the item being expanded?
            if ((bool)args.NewValue)
            {
                // Raise expanding event
                item.RaiseEvent(new RoutedEventArgs(ExpandingEvent, item));

                // Execute derived expanding handler
                item.OnExpanding();

                // Expand the item in the model
                item.Model?.Expand(item);

                // Raise expanded event
                item.RaiseEvent(new RoutedEventArgs(ExpandedEvent, item));

                // Execute derived expanded handler
                item.OnExpanded();
            }
            else
            {
                // Raise collapsing event
                item.RaiseEvent(new RoutedEventArgs(CollapsingEvent, item));

                // Execute derived collapsing handler
                item.OnCollapsing();

                // Collapse the item in the model
                item.Model?.Collapse(item);

                // Raise collapsed event
                item.RaiseEvent(new RoutedEventArgs(CollapsedEvent, item));

                // Execute derived collapsed handler
                item.OnCollapsed();
            }
        }

        private static DependencyProperty RegisterHasChildrenProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = false
            };

            // Register the property
            return DependencyProperty.Register(nameof(HasChildren), typeof(bool), typeof(TreeDataGridElement), metadata);
        }

        private static DependencyProperty RegisterIsExpandedProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                PropertyChangedCallback = OnIsExpandedChanged
            };

            // Register the property
            return DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(TreeDataGridElement), metadata);
        }

        private static DependencyProperty RegisterLevelProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = 0
            };

            // Register the property
            return DependencyProperty.Register(nameof(Level), typeof(int), typeof(TreeDataGridElement), metadata);
        }

        private static RoutedEvent RegisterExpandingEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Expanding), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridElement));
        }

        private static RoutedEvent RegisterExpandedEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Expanded), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridElement));
        }

        private static RoutedEvent RegisterCollapsingEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Collapsing), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridElement));
        }

        private static RoutedEvent RegisterCollapsedEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Collapsed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeDataGridElement));
        }

        public bool HasChildren
        {
            get { return (bool)GetValue(HasChildrenProperty); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            private set { SetValue(LevelProperty, value); }
        }

        public event RoutedEventHandler Expanding
        {
            add { AddHandler(ExpandingEvent, value); }
            remove { RemoveHandler(ExpandingEvent, value); }
        }

        public event RoutedEventHandler Expanded
        {
            add { AddHandler(ExpandedEvent, value); }
            remove { RemoveHandler(ExpandedEvent, value); }
        }

        public event RoutedEventHandler Collapsing
        {
            add { AddHandler(CollapsingEvent, value); }
            remove { RemoveHandler(CollapsingEvent, value); }
        }

        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(CollapsedEvent, value); }
            remove { RemoveHandler(CollapsedEvent, value); }
        }
    }

}
