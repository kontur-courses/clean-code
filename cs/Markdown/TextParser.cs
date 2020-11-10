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
                if (text[index] == '_')
                {
                    index++;
                    var indexOfClosingElement = FindIndexOfClosingElement('_', index, text);
                    var lengthOfCurrentElement = indexOfClosingElement - index;
                    splittedText.Add(new TextToken(index,lengthOfCurrentElement,TokenType.Emphasized,text.Substring(index,lengthOfCurrentElement)));
                    index = ++indexOfClosingElement;
                    
                }
            }

            return splittedText;
        }

        public int FindIndexOfClosingElement(char elementToFind,int startIndex, string text)
        {
            for (var index = startIndex; index < text.Length; index++)
            {
                if (text[index] == elementToFind)
                    return index;
            }
            throw new ArgumentException("No closing underlining");
        }
    }
}
