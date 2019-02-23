using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MinecraftToolsBoxSDK
{
	public class TreeDataGridModel : ObservableCollection<TreeDataGridElement>
	{
		public TreeDataGridFlatModel FlatModel { get; private set; }

		private List<TreeDataGridElement> itemCache;

		public TreeDataGridModel()
		{
			// Initialize the model
			itemCache = new List<TreeDataGridElement>();
			FlatModel = new TreeDataGridFlatModel();
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			// Process the event
			switch(args.Action)
			{
				case NotifyCollectionChangedAction.Add:

					// Process added item
					OnRootAdded(args.NewItems[0]);
					break;
			}
		}

		internal void Expand(TreeDataGridElement item)
		{
			// Do we need to expand the item?
			if (!FlatModel.ContainsKey(item) || !item.IsExpanded)
			{
				// We do not need to expand the item
				return;
			}

			// Clear the item cache
			itemCache.Clear();

			// Cache the flat children for the item
			CacheFlatChildren(item);

			// Get the insertion index for the item
			int index = (FlatModel.IndexOf(item) + 1);

			// Add the flat children to the flat model
			FlatModel.PrivateInsertRange(index, itemCache);
		}

		internal void Collapse(TreeDataGridElement item)
		{
			// Do we need to collapse the item?
			if (!FlatModel.ContainsKey(item))
			{
				// We do not need to collapse the item
				return;
			}

			// Get the collapse information
			int index = (FlatModel.IndexOf(item) + 1);
			int count = CountFlatChildren(item);

			// Remove the items from the flat model to collapse them
			FlatModel.PrivateRemoveRange(index, count);
		}

		internal void OnChildAdded(TreeDataGridElement child)
		{
			// Get the parent of the child
			TreeDataGridElement parent = child.Parent;

			// Check if the parent is expanded
			if (!FlatModel.ContainsKey(parent) || !parent.IsExpanded)
			{
				// We don't need to update the flat model
				return;
			}

			// Find the insertion index for the child into the flat model
			int index = FindFlatInsertionIndex(child);

			// Insert the child into the flat model
			FlatModel.PrivateInsert(index, child);

			// Expand the child
			Expand(child);
		}

		internal void OnChildReplaced(TreeDataGridElement oldChild, TreeDataGridElement child, int index)
		{

		}

		internal void OnChildRemoved(TreeDataGridElement child)
		{

		}

		internal void OnChildrenRemoved(TreeDataGridElement parent, IList children)
		{

		}

		private void OnRootAdded(object item)
		{
			// Verify the root item
			TreeDataGridElement root = TreeDataGridElement.VerifyItem(item);

			// Set the model for the root
			root.SetModel(this);

			// Find the index for insertion into the flat model
			int index = FindFlatInsertionIndex(root);

			// Insert the root into the flat model
			FlatModel.PrivateInsert(index, root);

			// Expand the root
			Expand(root);
		}

		private void CacheFlatChildren(TreeDataGridElement item)
		{
			// Iterate through all of the children within the item
			foreach (TreeDataGridElement child in item.Children)
			{
				// Add the child to the item cache
				itemCache.Add(child);

				// Is the child expanded?
				if (child.IsExpanded)
				{
					// Recursively cache the children within the child
					CacheFlatChildren(child);
				}
			}
		}

		private int CountFlatChildren(TreeDataGridElement item)
		{
			// Initialize child count
			int children = item.Children.Count;

			// Iterate through each child
			foreach (TreeDataGridElement child in item.Children)
			{
				// Is the child expanded?
				if (child.IsExpanded)
				{
					// Recursively count the children
					children += CountFlatChildren(child);
				}
			}

			// Return the number of flat children
			return children;
		}

		private int FindFlatInsertionIndex(TreeDataGridElement item)
		{
			// Get the search information
			TreeDataGridElement        parent    = item.Parent;
			IList<TreeDataGridElement> items     = ((parent != null) ? parent.Children : this);
			int                    index     = items.IndexOf(item);
			int                    lastIndex = (items.Count - 1);

			// Determine if the item is the last item in the items
			if (index < lastIndex)
			{
				// Return the insertion index using the items
				return FlatModel.IndexOf(items[(index + 1)]);
			}

			// Is the parent valid?
			else if (parent != null)
			{
				// Determine the number of flat children the parent has
				int children = CountFlatChildren(parent);

				// Return the insertion index using the number of flat children
				return (FlatModel.IndexOf(parent) + children);
			}
			else
			{
				// Return the flat model count
				return FlatModel.Count;
			}
		}
	}
}