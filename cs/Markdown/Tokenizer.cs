using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tokenizer
    {
        public readonly LinkedList<Token> tokens;

        private readonly Md markdown;
        private readonly string text;

        public Tokenizer(Md markdown, string text)
        {
            tokens = new LinkedList<Token>();
            this.text = text;
            this.markdown = markdown;
        }


        public Tokenizer ParseToTokens()
        {
            var shieldNext = false;
            StringBuilder builder = new();
            var tokenIndex = 0;
            tokens.Clear();
            var currentType = GetTokenOnIndex(0);
            for (var i = 0; i < text.Length; i++)
            {
                if (!shieldNext && markdown.IsStartOfTag(text[i]))
                {
                    if (builder.Length > 0)
                    {
                        AddToken(currentType, builder.ToString(), tokenIndex);
                        tokenIndex = i;
                    }

                    var tagBuilder = new StringBuilder();
                    for (; i < text.Length && markdown.IsStartOfTag(tagBuilder.ToString() + text[i]); i++)
                        tagBuilder.Append(text[i]);
                    AddToken(TokenType.Tag, tagBuilder.ToString(), tokenIndex);
                    tokenIndex = i;
                    builder.Clear();
                    continue;
                }

                var type = GetTokenOnIndex(i);
                if (currentType != type)
                {
                    AddToken(currentType, builder.ToString(), tokenIndex);
                    builder.Clear();
                    tokenIndex = i;
                    currentType = type;
                }

                shieldNext = !shieldNext && text[i] == '\\';

                if (!shieldNext || i + 1 >= text.Length || !markdown.IsShieldSymbol(text[i + 1]))
                    builder.Append(text[i]);
            }
            if (builder.Length > 0)
                AddToken(currentType, builder.ToString(), tokenIndex);
            return this;
        }

        private Token AddToken(TokenType type, string value, int index)
        {
            var token = new Token(type, value, index, tokens.Last?.Value);
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
            return TokenType.OtherSymbol;
        }
    }
}
