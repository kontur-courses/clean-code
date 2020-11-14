using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer : IEnumerable<Token>
    {
        private readonly LinkedList<Token> tokens = new LinkedList<Token>();
        private string text;

        public Tokenizer(string text)
        {
            this.text = text;
            ParseToToken();
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                ParseToToken();
            }
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ParseToToken()
        {
            tokens.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var currentType = GetTokenType(text.First());
            var builder = new StringBuilder();
            var tokenIndex = 0;
            for (var i = 0; i < text.Length; ++i)
            {
                var type = GetTokenType(text[i]);
                if (currentType != type)
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    builder.Clear();
                    tokenIndex = i;
                    currentType = type;
                }

                builder.Append(text[i]);
            }

            AddToken(currentType, builder.ToString(), tokenIndex);
        }

        private void AddToken(TokenType type, string value, int index)
        {
            var token = new Token(type, value, index);
            tokens.Last?.Value.SetNext(token);
            tokens.AddLast(token);
        }

        public Token First => tokens.First?.Value;

        private TokenType GetTokenType(char value)
        {
            if (value == '\n')
                return TokenType.BreakLine;
            if (char.IsWhiteSpace(value))
                return TokenType.Space;
            return char.IsLetterOrDigit(value)
                ? TokenType.Word
                : TokenType.SymbolSet;
        }
    }
}
