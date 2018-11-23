using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

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
                        controlTokensStack, result, tokens);
                
                result.Add(token);
            }

            return result;
        }

        private Token ProcessControlToken(Token token, int index,
            Rules rules, Stack<(Token, int)> controlTokensStack, 
            IList<Token> result, IReadOnlyList<Token> tokens)
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
            
            if (IsMarkingValid(tokens, token, index, rules, controlTokensStack))
                controlTokensStack.Push((token, index));

            return token;
        }

        private bool IsMarkingValid(IReadOnlyList<Token> tokens,
            Token token, int index, Rules rules, 
            Stack<(Token, int)> controlTokensStack) =>
            NextTokenNotStartWithSpace(tokens, token, index) &&
            NotInsideDigitsTokens(tokens, token, index) &&
            AllowInheritToken(tokens, token, index, rules, controlTokensStack);

        private bool AllowInheritToken(IReadOnlyList<Token> tokens,
            Token token, int index, Rules rules,
            Stack<(Token, int)> controlTokensStack)
        {
            if (controlTokensStack.Count == 0)
                return true;

            var (parentToken, _) = controlTokensStack.Peek();
            return !rules.ContainsProhibitInheritRuleFor(parentToken, token);
        }
            

        private bool NotInsideDigitsTokens(IReadOnlyList<Token> tokens, Token token, int index)
        {
            var previousToken = TryGetToken(tokens, index - 1);
            var nextToken = TryGetToken(tokens, index + 1);

            if (previousToken == null || nextToken == null)
                return true;

            var previousSymbol = previousToken.Value[previousToken.Length - 1];
            var nextSymbol = nextToken.Value[0];

            return !(char.IsDigit(previousSymbol) && char.IsDigit(nextSymbol));
        }

        private bool NextTokenNotStartWithSpace(IReadOnlyList<Token> tokens, Token token, int index)
        {
            var nextToken = TryGetToken(tokens, index + 1);
            return nextToken == null || !nextToken.Value.StartsWith(" ");
        }


        private Token TryGetToken(IReadOnlyList<Token> tokens, int index) =>
            index >= 0 && index < tokens.Count 
                ? tokens[index] 
                : null;
    }
}