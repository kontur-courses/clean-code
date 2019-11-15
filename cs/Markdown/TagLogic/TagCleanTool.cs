using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class TagCleanTool
    {
        public static List<Tag> GetCorrectMarkdownTags(string inputString, Dictionary<string, List<Tag>> tagsInInputString)
        {
            var sortedTags = tagsInInputString.Values
                .SelectMany(x => x)
                .OrderBy(x => x.Index)
                .ToList();

            return sortedTags
                .RemoveShieldedTags(inputString)
                .RemoveAllUnclosedTags()
                .RemoveIncorrectNestingTags()
                .ToList();
        }

        static IEnumerable<Tag> RemoveShieldedTags(this IEnumerable<Tag> tags, string inputString)
        {
            return tags
                .Where(tag => PreviousTagSymbol(tag) != '\\')
                .ToList();

            char PreviousTagSymbol(Tag tag) =>
                (tag.Index != 0) ? inputString[tag.Index - 1] : ' ';
        }

        static IEnumerable<Tag> RemoveAllUnclosedTags(this IEnumerable<Tag> sortedTags)
        {
            var result = new List<Tag>();

            var stack = new Stack<Tag>();
            foreach (var tag in sortedTags)
            {
                if (tag.Type == TagType.Opening)
                    stack.Push(tag);
                if (tag.Type == TagType.Closing && stack.Count != 0)
                {
                    var stackTopTag = stack.Peek();
                    if (stackTopTag.Symbol == tag.Symbol)
                    {
                        result.Add(stack.Pop());
                        result.Add(tag);
                    }
                }
            }

            return result.OrderBy(x => x.Index).ToList();
        }

        static IEnumerable<Tag> RemoveIncorrectNestingTags(this IEnumerable<Tag> sortedTags)
        {
            foreach (var tag in sortedTags) yield return tag;
        }
    }
}
