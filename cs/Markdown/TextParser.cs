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
                    var textElement = new StringBuilder();
                    for (var currentIndex = index; currentIndex < text.Length; currentIndex++)
                    {
                        if (text[currentIndex] == '_')
                        {
                            var textToken = new TextToken(index, currentIndex - index,TokenType.Emphasized,textElement.ToString());
                            splittedText.Add(textToken);
                            index = currentIndex;
                        }

                        textElement.Append(text[currentIndex]);
                    }
                }
            }

            return splittedText;
        }
    }
}
