using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagStreamRedactor
    {
        public static IEnumerable<Tag> RemoveEscapedTags(this IEnumerable<Tag> tags, string inputString)
        {
            return tags
                .Where(tag => PreviousTagSymbol(tag) != '\\');

            char PreviousTagSymbol(Tag tag) =>
                (tag.Index != 0) ? inputString[tag.Index - 1] : ' ';
        }

        public static IEnumerable<Tag> RemoveUnopenedTags(this IOrderedEnumerable<Tag> sortedTags)
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
                    if (stackTopTag.Designations == tag.Designations)
                    {
                        result.Add(stack.Pop());
                        result.Add(tag);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<Tag> RemoveIncorrectNestingTags(this IOrderedEnumerable<Tag> sortedTags)
        {
            var priorityStack = new Stack<int>();
            priorityStack.Push(int.MaxValue);
            foreach (var tag in sortedTags)
            {
                if (tag.Type == TagType.Opening && tag.Priority < priorityStack.Peek())
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
