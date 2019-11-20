using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdParser : Parser 
    {
        private string text;
        private readonly List<Token> tokens;
        private int parserPosition;
        private int nestedCount;

        public MdParser()
        {
            tokens = new List<Token>();
        }

        public override List<Token> GetTokens(string text)
        {
            this.text = text;
            var parserPosition = 0;
            var tokenTagType = TagType.GetTagType(text, parserPosition);
            while (parserPosition < text.Length)
            {
                AddToken(parserPosition, tokenTagType);
                nestedCount = 0;
                parserPosition += tokens.Last().Length;
                tokenTagType = parserPosition < text.Length
                    ? TagType.GetTagType(text, parserPosition)
                    : new DefaultTagType();
            }

            return tokens;
        }

        private void AddToken(int startPosition, TagType tokenTagType, bool hasNestedToken = false)
        {
            while (IsValidSymbol(tokenTagType, parserPosition))
            {
                if (tokenTagType is StrongTagType && TagType.GetTagType(text, parserPosition) is EmTagType)
                {
                    nestedCount++;
                    AddToken(parserPosition, new EmTagType(), true);
                }
                parserPosition++;
                if (parserPosition == text.Length)
                {
                    tokens.Add(new Token(startPosition, parserPosition - startPosition, tokenTagType, hasNestedToken,
                        nestedCount));
                    return;
                }
            }

            parserPosition += tokenTagType.GetOpenedTag(Tag.Markup.Md).Length;
            tokens.Add(new Token(startPosition, parserPosition - startPosition, tokenTagType, hasNestedToken,
                nestedCount));
        }

        private bool IsValidSymbol(TagType tokenTagType, int position)
        {
            return tokenTagType is DefaultTagType
                ? TagType.GetTagType(text, position) is DefaultTagType
                : !tokenTagType.IsClosedTag(text, position);
        }
    }
}