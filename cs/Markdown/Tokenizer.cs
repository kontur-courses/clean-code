using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public IEnumerable<Token> GetTokens(string expression, 
            IEnumerable<string> controlSequences)
        {
            var startIndex = 0;
            var tokens = new List<Token>();
            controlSequences = controlSequences.OrderByDescending(el => el).ToArray();
            
            while (startIndex < expression.Length)
            {
                var token = ReadToken(expression, startIndex, controlSequences);
                tokens.Add(token);
                startIndex += token.Length;
            }

            return tokens;
        }

        private Token ReadToken(string expression, int startIndex,
            IEnumerable<string> controlSequences)
        {
            var croppedExpression = expression.Substring(startIndex);
            var minIndex = int.MaxValue;
            string minIndexSequence = null;
            
            foreach (var sequence in controlSequences)
            {
                var index = croppedExpression.IndexOf(sequence, StringComparison.Ordinal);

                if (index == -1 || index >= minIndex) continue;
                
                minIndex = index;
                minIndexSequence = sequence;
            }
            
            if (minIndex == 0)
                return new Token(minIndexSequence, true);
                
            return minIndex == int.MaxValue
                ? new Token(croppedExpression, false) 
                : new Token(croppedExpression.Substring(0, minIndex), false);
        }
    }
}