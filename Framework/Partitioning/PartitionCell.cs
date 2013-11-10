using System;
using System.Collections.Generic;

namespace Sunfish.Views.Partitioning
{
	public class PartitionCell
	{

		internal HashSet<View> Views;

		public PartitionCell ()
		{
			Views = new HashSet<View>();
		}

		public void RemoveView(View viewToRemove)
		{
			Views.Remove (viewToRemove);
		}

		public void AddView(View viewToAdd)
		{
			Views.Add (viewToAdd);
		}

	}
}

