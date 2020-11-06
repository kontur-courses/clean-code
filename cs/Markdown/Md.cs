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
            foreach (var paragraph in md.Split('\n'))
            {
                var tags = ReadAllTags(paragraph).ShieldTags().ToArray();
                var singleTags = tags.Where(t => t is SingleTag && (t.Type != TagType.H1 || t.Start == 0));
                var pairedTags = tags
                    .GetPairedTags(md)
                    .RemoveTagsIntersection()
                    .RemoveStrongInItalics();
                
                result.Append(ChangeMdTagsToHtml(paragraph, singleTags.Concat(pairedTags).ToArray()));
                result.Append("\n");
            }

            return result.Remove(result.Length - 1, 1).ToString();
        }
        
        private static IEnumerable<Tag> ReadAllTags(string md)
        {
            for (var i = 0; i < md.Length; i++)
            {
                var type = ReadTagType(md, i, out var toSkip);
                if (type != null) yield return Tag.CreateTag(type.Value, i);
                i += toSkip - 1;
            }
        }
        
        private static TagType? ReadTagType(string md, int start, out int length)
        {
            length = 1;
            if (md[start] == '#') return TagType.H1;
            if (md[start] == '\\') return TagType.Shield;
            if (md[start] != '_') return null;
            if (md.Length > start + 1 && md[start + 1] == '_') length++;
            if(md.IsDigit(start - 1) || md.IsDigit(start + length)) return null;
            return length == 1 ? TagType.Em : TagType.Strong;
        }
        
        private static IEnumerable<Tag> ShieldTags(this IEnumerable<Tag> tags)
        {
            var shield = new Tag(TagType.Shield, -2);
            
            foreach (var tag in tags)
            {
                if (shield.Start + shield.Length == tag.Start) yield return shield;
                else if (tag.Type == TagType.Shield) shield = tag;
                else yield return tag;
            }
        }
        
        private static IEnumerable<PairedTag> GetPairedTags(this IEnumerable<Tag> tags, string md)
        {
            var waitingPair = new Dictionary<TagType, PairedTag>();
            
            foreach (var tag in tags.Where(t => t is PairedTag))
            {
                var pairedTag = tag as PairedTag;
                waitingPair.TryGetValue(tag.Type, out var first);
                if (first == null)
                {
                    if(!md.IsSpace(tag.Start + tag.Length))
                        waitingPair[tag.Type] = pairedTag;
                }
                else if(first.IsCorrectTagPair(pairedTag, md))
                {
                    yield return first;
                    pairedTag.IsOpening = false;
                    yield return pairedTag;
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
        
        private static IEnumerable<PairedTag> RemoveStrongInItalics(this IEnumerable<PairedTag> tags)
        {
            var isItalicsOpen = false;
            
            foreach (var tag in tags.OrderBy(t => t.Start))
            {
                if (isItalicsOpen && tag.Type == TagType.Strong) continue;
                
                yield return tag;
                if (tag.Type == TagType.Em) isItalicsOpen = !isItalicsOpen;
            }
        }

        private static string ChangeMdTagsToHtml(string md, IReadOnlyList<Tag> tags)
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
            
            if(startIndex < md.Length)
                result.Append(md.Substring(startIndex, md.Length - startIndex));
            if (tags.Count > 0 && tags[0].Type == TagType.H1)
                result.Append("</h1>");
            return result.ToString();
        }
    }
}