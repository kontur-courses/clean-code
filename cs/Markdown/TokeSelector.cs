using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public interface ITokenSelector
    {
        IEnumerable<Token> SelectTokens(LinkedList<Token> tokens);
    }
    
    public class TokenSelector : ITokenSelector
    {
        public List<Func<LinkedListNode<Token>, bool>> TokenDependency;
        
        public TokenSelector()
        {
            TokenDependency = new List<Func<LinkedListNode<Token>, bool>>();
        }
        
        public IEnumerable<Token> SelectTokens(LinkedList<Token> tokens)
        {
            var current = tokens.First;
            while (current != null)
            {
                if (current.Value.PosibleTag != null && !TokenDependency.Any(dep => !dep(current)))
                {
                    current.Value.IsClose = Tag.IsCloseTag(current);
                    current.Value.IsOpen = Tag.IsOpenTag(current);
                    yield return current.Value;
                    
                }
                current = current.Next;
            }
        }    
    }
}