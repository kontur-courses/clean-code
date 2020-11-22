using System.Collections;
using System.Collections.Generic;
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

        private void ParseToToken()
        {
            tokens.Clear();
            if (string.IsNullOrEmpty(text))
                return;
            var currentType = GetTokenOnIndex(0);
            var builder = new StringBuilder();
            var tokenIndex = 0;
            var shieldNext = false;

            bool Clear(int next, TokenType type)
            {
                builder.Clear();
                tokenIndex = next;
                currentType = type;
                return tokenIndex < text.Length;
            }

            for (var i = 0; i < text.Length; ++i)
            {
                if (!shieldNext && markdown.IsStartOfTag(text[i]))
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    var tagToken = ReadTagToken(i);
                    if (!Clear(i += tagToken.Length, GetTokenOnIndex(i)))
                        break;
                }
                var type = GetTokenOnIndex(i);
                if (currentType != type)
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    Clear(i, type);
                }

                shieldNext = !shieldNext && text[i] == '\\';

                if (!shieldNext || i + 1 >= text.Length || !markdown.IsShieldSymbol(text[i + 1]))
                    builder.Append(text[i]);
            }
            if (builder.Length > 0)
                AddToken(currentType, builder.ToString(), tokenIndex);
        }

        private Token ReadTagToken(int index)
        {
            var builder = new StringBuilder();
            var i = index;
            string currentValid;
            do
            {
                builder.Append(text[i]);
                currentValid = builder.ToString();
                ++i;
            } while (i < text.Length && markdown.ContainsTag(builder.ToString() + text[i], out _));
            return AddToken(TokenType.Tag, currentValid, index);
        }

        private Token ReadToken(int index)
        {
            return null;
        }

        private Token AddToken(TokenType type, string value, int index)
        {
            var token = new Token(type, value, index);
            tokens.Last?.Value.SetNext(token);
            tokens.AddLast(token);
            return token;
        }

        private TokenType GetTokenOnIndex(int index)
        {
            if (index >= text.Length)
                return TokenType.Undefined;
            var value = text[index];
            if (value == '\n')
                return TokenType.BreakLine;
            if (char.IsWhiteSpace(value))
                return TokenType.Space;
            if (char.IsNumber(value))
                return TokenType.Number;
            if (char.IsLetter(value))
                return TokenType.Word;
            return TokenType.SymbolSet;
        }
    }
}
