using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkdownLexer
    {
        private Tuple<string, TokenType>[] delimiters = 
        {
            Tuple.Create(BoldStyleElement.DoubleKeyword, BoldStyleElement.TokenType),
            Tuple.Create(ItalicStyleElement.DoubleKeyword, ItalicStyleElement.TokenType),
            Tuple.Create(@"\", TokenType.EscapingDelimiter)
        };        

        private int pos;
        private readonly string source;

        public MarkdownLexer(string source)
        {
            this.source = source;
        }

        public Token[] Tokenize()
        {
            var tokens = new List<Token>();
            var word = new StringBuilder();
            while (pos < source.Length)
            {
                if (TryFindDelimiter(out Tuple<string, TokenType> value))
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

        private bool TryFindDelimiter(out Tuple<string, TokenType> value)
        {
            foreach (var pair in delimiters)
            {
                var delimiter = pair.Item1;
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
            value = null;
            return false;
        }    
    }
}
  