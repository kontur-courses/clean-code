using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TextParser
    {
        public List<TextToken> GetTextTokens(string text)
        {
            if(text == null)
                throw new ArgumentException("string was null");

            var splittedText = new List<TextToken>();

            for (var index = 0; index < text.Length; index++)
            {
                switch (text[index])
                {
                    case '_':
                        index++;
                        var indexOfClosingElement = FindIndexOfClosingElement('_', index, text);
                        var lengthOfCurrentElement = indexOfClosingElement - index;
                        splittedText.Add(new TextToken(index, lengthOfCurrentElement, TokenType.Emphasized, text.Substring(index, lengthOfCurrentElement)));
                        index = ++indexOfClosingElement;
                        break;
                    default:
                        var indexOfEndText = FindIndexOfEndText(index,text);
                        var lengthOfCurrentText = indexOfEndText - index;
                        splittedText.Add(new TextToken(index, lengthOfCurrentText, TokenType.Text, text.Substring(index,lengthOfCurrentText)));
                        index = indexOfEndText;
                        break;

                }
            }
            return splittedText;
        }

        private static int FindIndexOfClosingElement(char elementToFind,int startIndex, string text)
        {
            for (var index = startIndex; index < text.Length; index++)
            {
                if (text[index] == elementToFind)
                    return index;
            }
            throw new ArgumentException("No closing underlining");
        }

        private static int FindIndexOfEndText(int startIndex, string text)
        {
            var specialSymbols = new char[]{'_', '#'};
            for (var index = startIndex; index < text.Length; index++)
            {
                if (specialSymbols.Contains(text[index]))
                    return index;
            }

            return text.Length;
        }
    }
}
