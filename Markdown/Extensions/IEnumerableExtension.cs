using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TSource> ForEachPairs<TSource>(
            this IList<TSource> source,
            Func<TSource, TSource, (TSource, TSource)> action)
        where TSource : class
        {
            if (source.Count < 2) return source;
            
            var dict = new Dictionary<Guid, TSource>();
            
            foreach (var (f, s) in source.AllPairs())
            {
                var a = action(
                    dict.ContainsKey(f.Item1) ? dict[f.Item1] : f.Item2, 
                    dict.ContainsKey(s.Item1) ? dict[s.Item1] : s.Item2);
                
                dict[f.Item1] = a.Item1;
                dict[s.Item1] = a.Item2;
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