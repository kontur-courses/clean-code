using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string md)
        {
            var result = new StringBuilder();
            foreach (var paragraph in md.Split(Environment.NewLine))
            {
                var tags = ReadAllTags(paragraph).ToArray();
                var singleTags = tags.GetCorrectSingleTags(paragraph);
                var pairedTags = tags
                    .GetPairedTags(paragraph)
                    .RemoveTagsIntersection()
                    .RemoveTagsInsidePair(TagType.Em, TagType.Strong);

                tags = singleTags.Concat(pairedTags).RemoveTagsInsidePair(TagType.Reference).ToArray();
                
                result.AppendLine(ChangeMdTagsToHtml(paragraph, tags));
            }

            return result.Remove(result.Length - Environment.NewLine.Length, Environment.NewLine.Length).ToString();
        }
        
        private static IEnumerable<Tag> ReadAllTags(string md)
        {
            for (var i = 0; i < md.Length; i++)
            {
                var tag = ReadTag(md, i, out var toSkip);
                if (tag != null) yield return tag;
                i += toSkip - 1;
            }
        }
        
        private static Tag ReadTag(string md, int start, out int length)
        {
            length = 1;
            switch (md[start])
            {
                case '#':
                    return new SingleTag(TagType.H1, start);
                case '\\':
                {
                    length++;
                    return md.IsTagStart(start + 1) ? new SingleTag(TagType.Shield, start) : null;
                }
                case '_':
                {
                    if (md.IsChar(start + 1, '_')) length++;
                    if (md.IsDigit(start - 1) || md.IsDigit(start + length)) return null;
                    var type = length == 1 ? TagType.Em : TagType.Strong;
                    return new PairedTag(type, start);
                }
                case '[':
                    return new ReferenceTag(start);
                case ']':
                    if (md.IsChar(start + 1, '(')) length++;
                    return new ReferenceTag(start);
                case ')':
                    return new ReferenceTag(start);
                default:
                    return null;
            }
        }

        private static IEnumerable<SingleTag> GetCorrectSingleTags(this IEnumerable<Tag> tags, string md)
        {
            var hasHeader = false;
            
            foreach (var tag in tags.OfType<SingleTag>())
            {
                if (tag.Type == TagType.H1)
                {
                    if (tag.Start != 0) continue;
                    hasHeader = true;
                    yield return tag;
                }
                else
                {
                    yield return tag;
                }
            }

            if (hasHeader) yield return new SingleTag(TagType.H1, md.Length, false);
        }
        
        private static IEnumerable<PairedTag> GetPairedTags(this IEnumerable<Tag> tags, string md)
        {
            var waitingPair = new Dictionary<TagType, PairedTag>();
            
            foreach (var tag in tags.OfType<PairedTag>())
            {
                waitingPair.TryGetValue(tag.Type, out var first);
                if (first == null)
                {
                    if(!md.IsSpace(tag.Start + tag.Length))
                        waitingPair[tag.Type] = tag;
                }
                else if(first.TryMatchTagPair(tag, md))
                {
                    yield return first;
                    yield return tag;
                    waitingPair[tag.Type] = null;
                }
            }
        }
        
        private static IEnumerable<PairedTag> RemoveTagsIntersection(this IEnumerable<PairedTag> tags)
        {
            var tagsToClose = new Stack<PairedTag>();
            
            foreach (var tag in tags.OrderBy(t => t.Start))
            {
                if (tagsToClose.Count == 0 || tagsToClose.Peek().Type != tag.Type)
                {
                    tagsToClose.Push(tag);
                }
                else
                {
                    yield return tagsToClose.Pop();
                    yield return tag;
                }
            }
        }

        private static IEnumerable<Tag> RemoveTagsInsidePair(this IEnumerable<Tag> tags,
            TagType pairType,
            TagType? toDelete = null)
        {
            var isPairOpen = false;
            
            foreach (var tag in tags.OrderBy(t => t.Start))
            {
                if (tag.Type == pairType) isPairOpen = !isPairOpen;
                else if (isPairOpen && (toDelete == null || tag.Type == toDelete.Value)) continue;
                
                yield return tag;
            }
        }

        private static string ChangeMdTagsToHtml(string md, IEnumerable<Tag> tags)
        {
            var result = new StringBuilder();
            var startIndex = 0;
            
            foreach (var tag in tags.OrderBy(t => t.Start))
            {
                var length = tag.Start - startIndex;
                result.Append(md.Substring(startIndex, length));
                result.Append(tag.ToHtml());
                startIndex = tag.Start + tag.Length;
            }

            if (startIndex < md.Length) result.Append(md.Substring(startIndex, md.Length - startIndex));
            return result.ToString();
        }
    }
}