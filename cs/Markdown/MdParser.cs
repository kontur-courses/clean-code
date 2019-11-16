using System.Collections.Generic;

namespace Markdown
{
    public class MdParser
    {
        private readonly string text;

        public MdParser(string text)
        {
            this.text = text;
        }

        public IEnumerable<Token> GetTokens()
        {
            var tokens = new List<Token>();
            var startPosition = 0;
            var currentPosition = 0;
            var tokenTagType = TagType.GetTagType(text, startPosition);
            
            while (currentPosition < text.Length)
            {
                var symbolType = TagType.GetTagType(text, currentPosition);

                if (tokenTagType is DefaultTagType)
                {
                    if (symbolType is EmTagType || symbolType is StrongTagType)
                    {
                        tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                        startPosition = currentPosition;
                        tokenTagType = symbolType;
                    }
                    else
                    {
                        currentPosition++;
                        if (currentPosition == text.Length)
                            tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                    }
                }
                else if (tokenTagType is EmTagType)
                {
                    if (symbolType is EmTagType)
                    {
                        if (EmTagType.IsClosedTag(text, currentPosition))
                        {
                            currentPosition++;
                            tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                            startPosition = currentPosition;
                            tokenTagType = currentPosition < text.Length
                                ? TagType.GetTagType(text, currentPosition)
                                : new DefaultTagType();
                        }
                        else
                        {
                            currentPosition++;
                            if (currentPosition == text.Length)
                                tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                        }
                    }
                    else
                    {
                        currentPosition += symbolType.HtmlOpeningTag == string.Empty ? 1 : symbolType.HtmlOpeningTag.Length;
                        if (currentPosition == text.Length)
                            tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                    }
                }
                else if (tokenTagType is StrongTagType)
                {
                    if (symbolType is StrongTagType)
                    {
                        if (StrongTagType.IsClosedTag(text, currentPosition))
                        {
                            currentPosition += 2;
                            tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                            startPosition = currentPosition;
                            tokenTagType = currentPosition < text.Length
                                ? TagType.GetTagType(text, currentPosition)
                                : new DefaultTagType();
                        }
                        else
                        {
                            currentPosition += 2;
                            if (currentPosition == text.Length)
                                tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                        }
                    }
                    else
                    {
                        currentPosition++;
                        if (currentPosition == text.Length)
                            tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                    }
                }
            }

            return tokens;
        }
    }
}