﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextFreeGrammars {
	internal static class Helpers {
		public static IEnumerable<T> LookupEnumerable<TKey, T>	(
			this IDictionary<TKey, ISet<T>> dictionary,
			TKey key
		) {
			ISet<T> retval;
			if (dictionary.TryGetValue(key, out retval)) {
				return retval;
			}
			return Enumerable.Empty<T>();
		}
	}
}
