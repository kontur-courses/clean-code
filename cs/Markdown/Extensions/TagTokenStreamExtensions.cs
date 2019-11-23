using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownTags;

namespace Markdown
{
    internal static class TagTokenStreamExtensions
    {
        public static IEnumerable<TagToken> RemoveEscapedTags(this IEnumerable<TagToken> tagTokens, string inputString)
        {
            return tagTokens
                .Where(tag => PreviousTagSymbol(tag) != '\\');

            char PreviousTagSymbol(TagToken tag) =>
                (tag.Index != 0) ? inputString[tag.Index - 1] : ' ';
        }

        public static IEnumerable<TagToken> RemoveUnpairedTags(this IEnumerable<TagToken> sortedTagTokens)
        {
            var result = new List<TagToken>();
            var tagTokenStacks = new Dictionary<MarkdownTagInfo, Stack<TagToken>>();

            foreach (var tagToken in sortedTagTokens)
            {
                if(!tagTokenStacks.ContainsKey(tagToken.MarkdownTagInfo))
                    tagTokenStacks[tagToken.MarkdownTagInfo] = new Stack<TagToken>();
                
                if (tagToken.TokenType == TagTokenType.Opening)
                    tagTokenStacks[tagToken.MarkdownTagInfo].Push(tagToken);
                if (tagToken.TokenType == TagTokenType.Closing
                    && tagTokenStacks[tagToken.MarkdownTagInfo].Count != 0)
                {
                    result.Add(tagTokenStacks[tagToken.MarkdownTagInfo].Pop());
                    result.Add(tagToken);
                }
            }

            return result.OrderBy(tag => tag.Index);
        }

        public static IEnumerable<TagToken> RemoveIncorrectNestingTags(this IEnumerable<TagToken> sortedTagTokens)
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
