using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook.Web.Code
{
	public static class EnumerableExtensions
	{
		public static SortedDictionary<TKey, TValue> ToSortedDictionary<TItem, TKey, TValue>
		(
			this IEnumerable<TItem> items,
			Func<TItem, TKey> keySelector,
			Func<TItem, TValue> valueSelector,
			IComparer<TKey> keyComparer = null
		)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}

			if (keySelector == null)
			{
				throw new ArgumentNullException(nameof(keySelector));
			}

			if (valueSelector == null)
			{
				throw new ArgumentNullException(nameof(valueSelector));
			}

			var sortedDictionary = new SortedDictionary<TKey, TValue>(keyComparer);

			foreach (var item in items)
			{
				sortedDictionary.Add(keySelector(item), valueSelector(item));
			}

			return sortedDictionary;
		}

	}
}