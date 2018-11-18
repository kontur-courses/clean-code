using System.Collections.Generic;
using Markdown.Element;
using Markdown.Extensions;

namespace Markdown.Token
{
    public class TokenParser
    {
        private static IElement textElement = new TextElement();

        public static IEnumerable<Token> Parse(string markdown, IElement element)
        {
            var result = new List<Token>();
            var currentIndex = 0;

            while (currentIndex < markdown.Length)
            {
                var openIndex = element.GetOpenIndex(markdown, currentIndex);
                var closeIndex = element.GetCloseIndex(markdown, openIndex);

                if (openIndex == -1 || closeIndex == -1)
                {
                    result.Add(new Token(markdown.Substring(currentIndex), textElement)); 
                    break;
                }

                if (openIndex != currentIndex)
                {
                    result.Add(CreateToken(markdown, currentIndex, openIndex, textElement));
                }

                result.Add(CreateToken(markdown, openIndex, closeIndex, element));
                currentIndex = closeIndex + element.Indicator.Length;
            }

            return result;
        }

        private static Token CreateToken(string markdown, int openIndex, int closeIndex, IElement element)
        {
            var content = markdown.Substring(openIndex + element.Indicator.Length,
                closeIndex - openIndex - element.Indicator.Length);

            return new Token(content, element);
        }
    }
}