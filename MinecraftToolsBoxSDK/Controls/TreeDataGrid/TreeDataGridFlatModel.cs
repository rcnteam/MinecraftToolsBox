using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MinecraftToolsBoxSDK
{
	public class TreeDataGridFlatModel : ObservableCollection<TreeDataGridElement>
	{
		private const string ModificationError = "The collection cannot be modified by the user.";

		private bool                     modification;
		private HashSet<TreeDataGridElement> keys;

		public TreeDataGridFlatModel()
		{
			// Initialize the model
			keys = new HashSet<TreeDataGridElement>();
		}

		internal bool ContainsKey(TreeDataGridElement item)
		{
			// Return a value indicating if the item is within the model
			return keys.Contains(item);
		}

		internal void PrivateInsert(int index, TreeDataGridElement item)
		{
			// Set the modification flag
			modification = true;

			// Add the item to the model
			Insert(index, item);

			// Add the item to the keys
			keys.Add(item);

			// Clear the modification flag
			modification = false;
		}

		internal void PrivateInsertRange(int index, IList<TreeDataGridElement> items)
		{
			// Set the modification flag
			modification = true;

			// Iterate through all of the children within the items
			foreach (TreeDataGridElement child in items)
			{
				// Add the child to the model
				Insert(index++, child);

				// Add the child to the keys
				keys.Add(child);
			}

			// Clear the modification flag
			modification = false;
		}

		internal void PrivateRemoveRange(int index, int count)
		{
			// Set the modification flag
			modification = true;

			// Iterate through all of the items to remove from the model
			for (int itemIndex = 0; itemIndex < count; itemIndex++)
			{
				// Remove the item from the keys
				keys.Remove(Items[index]);

				// Remove the item from the model
				RemoveAt(index);
			}

			// Clear the modification flag
			modification = false;
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			// Is the modification flag set?
			if (!modification)
			{
				// The collection is for internal use only
				throw new InvalidOperationException(ModificationError);
			}

			// Call base method
			base.OnCollectionChanged(args);
		}
	}
}