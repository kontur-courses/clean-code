using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownProcessor
{
    public class Tokenizer
    {
        private Dictionary<string, TokenType> tags = new Dictionary<string, TokenType>
        {
            {"_", TokenType.ItalicTag},
            {"__", TokenType.BoldTag},
            {"# ", TokenType.HeaderTag}
        };
        
        private int maxTagLength;
        
        
        public Tokenizer()
        {
            maxTagLength = tags.Keys.OrderByDescending(x => x.Length).First().Length;
        }

        public IEnumerable<Token> GetTokens(string input)
        {
            var lastReturnedIndex = 0;
            var index = 0;
            while (index < input.Length)
            {
                if (IsTagFound(input, index))
                {
                    if (index > lastReturnedIndex)
                    {
                        yield return new Token(input.Substring(lastReturnedIndex + 1, index - lastReturnedIndex - 1), TokenType.Text);
                        lastReturnedIndex += index - lastReturnedIndex - 1;
                    }

                    foreach (var tag in tags.Where(tag => input.StartsWith(tag.Key)))
                    {
                        yield return new Token(tag.Key, tag.Value);
                        lastReturnedIndex = index - 1 + tag.Key.Length;
                    }
                }

                index++;
            }

            if (index - 1 > lastReturnedIndex)
            {
                if (lastReturnedIndex == 0)
                    yield return new Token(input, TokenType.Text);
                else
                    yield return new Token(input.Substring(lastReturnedIndex + 1), TokenType.Text);
            }
        }

        private bool IsTagFound(string line, int index)
        {
            return tags.Keys.Any(line.Substring(index).StartsWith);
        }
    }
}