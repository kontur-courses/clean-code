using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenSelector : ITokenSelector
    {
        private readonly List<Func<LinkedListNode<Token>, bool>> TokenDependencies;

        public TokenSelector()
        {
            TokenDependencies = new List<Func<LinkedListNode<Token>, bool>>();
        }

        public IEnumerable<Token> SelectTokens(LinkedList<Token> tokens)
        {
            var current = tokens.First;
            while (current != null)
            {
                if (current.Value.PosibleTag != null && !TokenDependencies.Any(dep => !dep(current)))
                {
                    current.Value.IsClose = Tag.IsCloseTag(current);
                    current.Value.IsOpen = Tag.IsOpenTag(current);
                    yield return current.Value;
                }

                current = current.Next;
            }
        }

        public void AddDependency(Func<LinkedListNode<Token>, bool> dependency)
        {
            TokenDependencies.Add(dependency);
        }
    }
}