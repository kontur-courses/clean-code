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
        public static void RemoveUnpaired(this List<TagToken> tokens)
        {
            var stack = new Stack<TagToken>();

            foreach (var token in tokens)
            {
                if (!stack.Any())
                {
                    stack.Push(token);
                    continue;
                }

                var previousToken = stack.Peek();

                if (previousToken.Type == token.Type && previousToken.Order == SubTagOrder.Opening)
                    stack.Pop();
                else
                    stack.Push(token);
            }

            foreach (var unpairedToken in stack)
                tokens.Remove(unpairedToken);
        }
    }
}
