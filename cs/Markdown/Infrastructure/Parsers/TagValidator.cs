using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class TagValidator : ITagValidator
    {
        private readonly string text;

        public TagValidator(string text)
        {
            this.text = text;
        }
        /// <summary>
        ///     Validate interaction of tags according to rules
        /// </summary>
        public IEnumerable<TagInfo> GetValidTags(IEnumerable<TagInfo> tagInfos)
        {
            tagInfos = FilterEscaped(tagInfos);
            tagInfos = FilterIntersections(tagInfos, text).OrderBy(tagInfo => tagInfo.Offset);
            tagInfos = FilterDoubleInSingle(tagInfos);
            tagInfos = FilterEmptyTags(tagInfos, text);

            return tagInfos;
        }
        
        
        private IEnumerable<TagInfo> FilterDoubleInSingle(IEnumerable<TagInfo> tagInfos)
        {
            var isUnderscoreOpen = false;
            foreach (var tagInfo in tagInfos)
            {
                if (tagInfo.Tag.Style == Style.Angled)
                    isUnderscoreOpen = !isUnderscoreOpen;

                if (tagInfo.Tag.Style == Style.Bold && isUnderscoreOpen)
                    continue;

                yield return tagInfo;
            }
        }

        private IEnumerable<TagInfo> FilterEmptyTags(IEnumerable<TagInfo> tagInfos, string text)
        {
            var toSkip = new List<TagInfo>();

            var enumerable = tagInfos.ToList();
            foreach (var (previous, current) in enumerable.Zip(enumerable.Skip(1)))
                if (current.Closes(previous, text) && current.Follows(previous))
                {
                    toSkip.Add(previous);
                    toSkip.Add(current);
                }

            return enumerable.Where(info => !toSkip.Contains(info));
        }

        private IEnumerable<TagInfo> FilterEscaped(IEnumerable<TagInfo> tagInfos)
        {
            using var enumerator = tagInfos.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var tagInfo = enumerator.Current;
                if (tagInfo.Tag.Style == Style.Escape)
                {
                    enumerator.MoveNext();
                    if (enumerator.Current.Follows(tagInfo))
                        yield return tagInfo;
                }
                else
                {
                    yield return tagInfo;
                }
            }
        }

        private IEnumerable<TagInfo> FilterIntersections(IEnumerable<TagInfo> tagInfos, string text)
        {
            var unclosed = new Stack<TagInfo>();
            foreach (var tagToCheck in tagInfos)
                if (unclosed.TryPeek(out var tagToClose) && tagToCheck.Closes(tagToClose, text))
                {
                    yield return unclosed.Pop();
                    yield return tagToCheck;
                }
                else
                {
                    unclosed.Push(tagToCheck);
                }
        }
    }
}