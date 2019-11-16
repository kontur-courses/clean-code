using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class TagCleanTool
    {
        public static List<Tag> GetCorrectMarkdownTags(string inputString, Dictionary<string, List<Tag>> tagsInInputString)
        {
            if (inputString == null) throw  new ArgumentNullException();
            if (tagsInInputString.Count == 0) return new List<Tag>();

            var sortedTags = tagsInInputString.Values
                .SelectMany(x => x);

            return sortedTags
                .RemoveShieldedTags(inputString)
                .OrderBy(tag => tag.Index)
                .RemoveUnclosedTags()
                .RemoveIncorrectNestingTags()
                .ToList();
        }

        static IEnumerable<Tag> RemoveShieldedTags(this IEnumerable<Tag> tags, string inputString)
        {
            return tags
                .Where(tag => PreviousTagSymbol(tag) != '\\');

            char PreviousTagSymbol(Tag tag) =>
                (tag.Index != 0) ? inputString[tag.Index - 1] : ' ';
        }

        static IOrderedEnumerable<Tag> RemoveUnclosedTags(this IOrderedEnumerable<Tag> sortedTags)
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

            return result.OrderBy(x => x.Index);
        }

        static IEnumerable<Tag> RemoveIncorrectNestingTags(this IOrderedEnumerable<Tag> sortedTags)
        {
            var priorityStack = new Stack<int>();
            priorityStack.Push(Int32.MaxValue);
            foreach (var tag in sortedTags)
            {
                if (tag.Type == TagType.Opening && tag.Priority <= priorityStack.Peek())
                {
                    priorityStack.Push(tag.Priority);
                    yield return tag;
                }

                if (tag.Type == TagType.Closing && tag.Priority == priorityStack.Peek())
                {
                    priorityStack.Pop();
                    yield return tag;
                }
            }
        }
    }
}
