using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace AddressBook.Web.Code
{
	public static class ObjectCacheExtensions
	{
		public static TItem GetOrGenerate<TItem>(this ObjectCache cache, string cacheKey, TimeSpan relativeExpiration, Func<TItem> generate)
		{
			var value = cache[cacheKey];

			if (value == null)
			{
				value = generate();

				if (value != null)
				{
					cache.Set(cacheKey, value, DateTimeOffset.UtcNow.Add(relativeExpiration));
				}
			}

			return (TItem)value;
		}

	}
}