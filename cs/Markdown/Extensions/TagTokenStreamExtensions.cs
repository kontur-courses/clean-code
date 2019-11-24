using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownTags;

namespace Markdown
{
    internal static class TagTokenStreamExtensions
    {
        public static IEnumerable<TagToken> RemoveEscapedTagTokens(this IEnumerable<TagToken> tagTokens, string inputString)
        {
            return tagTokens
                .Where(tag => PreviousTagTokenSymbol(tag) != '\\');

            char PreviousTagTokenSymbol(TagToken tag) =>
                (tag.Index != 0) ? inputString[tag.Index - 1] : ' ';
        }

        public static IEnumerable<TagToken> RemoveIncorrectTagSequences(this IEnumerable<TagToken> sortedTagTokens)
        {
            var result = new List<TagToken>();
            
            var stack = new Stack<TagToken>();
            foreach (var tag in sortedTagTokens)
            {
                if (tag.TokenType == TagTokenType.Opening)
                    stack.Push(tag);
                if (tag.TokenType == TagTokenType.Closing && stack.Count != 0)
                {
                    var stackTopTag = stack.Peek();
                    if (stackTopTag.MarkdownTagInfo.Equals(tag.MarkdownTagInfo))
                    {	
                        result.Add(stack.Pop());	
                        result.Add(tag);	
                    }
                }
            }

            return result.OrderBy(tag => tag.Index);
        }

        public static IEnumerable<TagToken> RemoveIncorrectNestingTagTokens(this IEnumerable<TagToken> sortedTagTokens)
        {
            var priorityStack = new Stack<int>();
            priorityStack.Push(int.MaxValue);
            foreach (var tag in sortedTagTokens)
            {
                if (tag.TokenType == TagTokenType.Opening && tag.MarkdownTagInfo.Priority < priorityStack.Peek())
                {
                    priorityStack.Push(tag.MarkdownTagInfo.Priority);
                    yield return tag;
                }

                if (tag.TokenType == TagTokenType.Closing && tag.MarkdownTagInfo.Priority == priorityStack.Peek())
                {
                    priorityStack.Pop();
                    yield return tag;
                }
            }
        }
    }
}
