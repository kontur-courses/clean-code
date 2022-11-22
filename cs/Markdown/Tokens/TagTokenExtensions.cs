using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public static class TagTokenExtensions
    {
        //public static List<TagToken> RemoveEscaped(this List<TagToken> tokens, string text)
        //{

        //}

        public static List<TagToken> RemoveUnpaired(this List<TagToken> tokens)
        {
            var stack = new Stack<TagToken>();

            foreach (var token in tokens)
            {
                var tagToken = token as TagToken;

                if (tagToken == null)
                    continue;

                if (!stack.Any())
                {
                    stack.Push(tagToken);
                    continue;
                }

                var previousToken = stack.Peek() as TagToken;

                if (previousToken == null)
                    continue;

                if (previousToken.TagType == tagToken.TagType && previousToken.Order == SubTagOrder.Opening)
                    stack.Pop();
                else
                    stack.Push(tagToken);
            }

            foreach (var unpairedToken in stack)
                tokens.Remove(unpairedToken);

            return tokens;
        }
    }
}
