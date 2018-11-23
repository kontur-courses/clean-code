using System.Collections.Generic;
using System.Diagnostics;

namespace Markdown.HTMLTransducer
{
    public class Transducer
    {
        public List<Token> Transform(List<Token> tokens,
            Rules rules)
        {
            var escapeMode = false;
            var result = new List<Token>();
            var controlTokensStack = new Stack<(Token, int)>();

            for (int index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                
                if (escapeMode)
                {
                    escapeMode = false;
                    result.Add(token);
                    continue;
                }

                if (rules.IsEscape(token))
                {
                    escapeMode = true;
                    continue;
                }

                if (rules.ContainsRuleFor(token))
                    token = ProcessControlToken(token, index, rules, 
                        controlTokensStack, result);
                
                result.Add(token);
            }

            return result;
        }

        private Token ProcessControlToken(Token token, int index,
            Rules rules, Stack<(Token, int)> controlTokensStack, 
            IList<Token> result)
        {
            var isOpening = true;
            
            if (controlTokensStack.Count != 0)
            {
                var (openedToken, openedTokenIndex) = controlTokensStack.Peek();
                if (Equals(token, openedToken))
                {
                    controlTokensStack.Pop();
                    result[openedTokenIndex] = rules.PerformFor(openedToken, false);
                    token = rules.PerformFor(token, true);
                    isOpening = false;
                }
            }
            
            if (isOpening)
                controlTokensStack.Push((token, index));

            return token;
        }
    }
}