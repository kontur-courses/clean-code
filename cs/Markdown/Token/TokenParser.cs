using System.Collections.Generic;
using System.Linq;
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

            if (element is HeaderHtmlElement headerHtmlElement)
            {
                var depth = markdown.TakeWhile(e => e.ToString() == element.Indicator).ToArray();

                if (depth.Length > 0 && depth.Length <= 6 && markdown[depth.Length] == ' ')
                {
                    result.Add(CreateToken(markdown, depth.Length, markdown.Length, 
                        new HeaderHtmlElement(headerHtmlElement.HtmlTag, depth.Length, headerHtmlElement.Indicator)));
                    return result;
                }
            }

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