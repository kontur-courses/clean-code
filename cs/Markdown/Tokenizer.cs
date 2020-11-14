using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer : IEnumerable<Token>
    {
        private readonly LinkedList<Token> tokens = new LinkedList<Token>();
        private readonly Md markdown;
        private string text;

        public Tokenizer(Md markdown, string text)
        {
            this.text = text;
            this.markdown = markdown;
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

        public Token First => tokens.First?.Value;

        public IEnumerator<Token> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Тут просто неформатированный выброс мыслей, всё очень страшно
        private void ParseToToken()
        {
            tokens.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var currentType = GetTokenType(text.First());
            var builder = new StringBuilder();
            var tokenIndex = 0;
            var shieldNext = false;
            for (var i = 0; i < text.Length; ++i)
            {
                if (!shieldNext && markdown.IsStartOfTag(text[i]))
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    var tagToken = ReadTagToken(i);
                    builder.Clear();
                    tokenIndex = i = tagToken.StartIndex + tagToken.Length;
                    if (i >= text.Length)
                        break;
                    currentType = GetTokenType(text[i]);
                }
                var type = GetTokenType(text[i]);
                if (currentType != type)
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    builder.Clear();
                    tokenIndex = i;
                    currentType = type;
                }

                if (!shieldNext && text[i] == '\\')
                    shieldNext = true;
                else
                    shieldNext = false;
                if (i + 1 < text.Length && shieldNext && markdown.IsShieldSymbol(text[i + 1]))
                    i = i;
                else
                    builder.Append(text[i]);
            }

            AddToken(currentType, builder.ToString(), tokenIndex);
        }

        private Token ReadTagToken(int index)
        {
            var builder = new StringBuilder();
            var i = index;
            string currentValid = null;
            do
            {
                builder.Append(text[i]);
                currentValid = builder.ToString();
                ++i;
            } while (i < text.Length && markdown.ContainsTag(builder.ToString() + text[i], out _));
            return AddToken(TokenType.Tag, currentValid, index);
        }

        private Token AddToken(TokenType type, string value, int index)
        {
            var token = new Token(type, value, index);
            tokens.Last?.Value.SetNext(token);
            tokens.AddLast(token);
            return token;
        }

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
