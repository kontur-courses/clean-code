using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    internal static class EnumerableExtension
    {
        public static IEnumerable<TSource> ForEachPairs<TSource>(
            this IList<TSource> source,
            Func<TSource, TSource, (TSource, TSource)> action)
        where TSource : class
        {
            if (source.Count < 2) return source;
            
            var dict = new Dictionary<Guid, TSource>();
            
            foreach (var ((firstGuid, firstItem), (secondGuid, secondItem)) in source.AllPairs())
            {
                var (transformedFirstItem, transformedSecondItem) = action(
                    dict.ContainsKey(firstGuid) ? dict[firstGuid] : firstItem, 
                    dict.ContainsKey(secondGuid) ? dict[secondGuid] : secondItem);
                
                dict[firstGuid] = transformedFirstItem;
                dict[secondGuid] = transformedSecondItem;
            }

            return dict.Select(x => x.Value);
        }

        private static IEnumerable<((Guid, TSource), (Guid, TSource))> AllPairs<TSource>(this IList<TSource> source)
        {
            var fixedSource = source.Select(x => (Guid.NewGuid(), x)).ToList();

            for (var i = 0; i < fixedSource.Count - 1; i++)
                for (var j = i + 1; j < fixedSource.Count; j++)
                    yield return (fixedSource[i], fixedSource[j]);
        }
    }
}