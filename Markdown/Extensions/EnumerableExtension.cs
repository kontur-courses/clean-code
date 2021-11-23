using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    internal static class EnumerableExtension
    {
        public static IEnumerable<SegmentsCollection> ForEachPairs(this IList<SegmentsCollection> source,
            Func<SegmentsCollection, SegmentsCollection, (SegmentsCollection, SegmentsCollection)> action)
        {
            if (source.Count < 2) return source;
            
            var dict = new Dictionary<Guid, List<SegmentsCollection>>();
            
            foreach (var ((firstGuid, firstItem), (secondGuid, secondItem)) in source.AllPairs())
            {
                var (transformedFirstItem, transformedSecondItem) = action(firstItem, secondItem);
                if (!dict.ContainsKey(firstGuid)) dict[firstGuid] = new List<SegmentsCollection>();
                if (!dict.ContainsKey(secondGuid)) dict[secondGuid] = new List<SegmentsCollection>();
                dict[firstGuid].Add(transformedFirstItem);
                dict[secondGuid].Add(transformedSecondItem);
            }

            var d = dict.Select(x => SegmentsCollection.GetIntersection(x.Value)).ToList();
            return d;
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