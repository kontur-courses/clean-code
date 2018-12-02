using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkdownLexer
    {
        private readonly (string delimiter, TokenType tokenType)[] delimiters;              
        private int pos;
        private readonly string source;

        public MarkdownLexer(string source, (string delimiter, TokenType tokenType)[] delimiters)
        {
            this.source = source;
            this.delimiters = delimiters;
        }

        public Token[] Tokenize()
        {
            var tokens = new List<Token>();
            var word = new StringBuilder();
            while (pos < source.Length)
            {
                if (TryFindDelimiter(out var value))
                {
                    if(word.Length != 0)
                        tokens.Add(new Token(word.ToString(), TokenType.SimpleWord));
                    word.Clear();
                    tokens.Add(new Token(value.Item1, value.Item2));
                }
                else
                {
                    word.Append(source[pos]);
                    pos++;
                }                 
            }
            if (word.Length != 0)
                tokens.Add(new Token(word.ToString(), TokenType.SimpleWord));
            return tokens.ToArray();
        }

        private bool TryFindDelimiter(out (string, TokenType) value)
        {
            foreach (var pair in delimiters)
            {
                var delimiter = pair.delimiter;
                if(delimiter.Length + pos > source.Length)
                    continue;
                var compSubStr = source.Substring(pos, delimiter.Length);
                if (delimiter == compSubStr)
                {
                    pos += delimiter.Length;
                    value = pair;
                    return true;
                }
            }
            value = ("",TokenType.SimpleWord);
            return false;
        }    
    }
}
  