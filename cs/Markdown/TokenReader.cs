using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenReader
    {
        private MatchingTree<char> matchingTree;
        private StringBuilder stringBuilder;
        private MathcingNode<char> driver; 

        public TokenReader(IEnumerable<string> tokens)
        {
            stringBuilder = new StringBuilder();
            matchingTree = new MatchingTree<char>(tokens);
        }

        public IEnumerable<Token> ReadTokens(String source)
        {
            
            driver = matchingTree.GetHead();
            var str = source.ToSpecialLinkedList();
            foreach (var ch in str)
            {

                if (ch == '\\')
                {
                    yield return ConstructToken();
                    yield return new Token(str.Take().ToString(),false);
                    continue;
                }
                    
                if (driver.GetNode(ch) != null)
                {
                    driver = driver.GetNode(ch);
                    stringBuilder.Append(ch);
                    
                }
                else if (matchingTree.IsTaged(driver))
                {
                    str.PushFront(ch);
                    yield return ConstructToken();
                    Reset();
                    
                }
                else
                {
                    stringBuilder.Append(ch);
                    driver = matchingTree.GetHead();
                    yield return ConstructToken();    
                    Reset();
                }
            }

            if (stringBuilder.Length != 0)
                yield return new Token(stringBuilder.ToString(),false);
        }

        private Token ConstructToken()
        {
            return new Token(stringBuilder.ToString(),matchingTree.IsTaged(driver));
        }

        private void Reset()
        {
            driver = matchingTree.GetHead();
            stringBuilder.Clear();
        }
    }
}