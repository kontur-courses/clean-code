using System;
using System.Linq;

namespace MarkDown
{
    public class Md
    {
        public string Render(string input)
        {
            var paragraps = TextPreparer.PrepareText(input);
            var preparedParagraphs = paragraps.Select(x => TextPreparer.PrepareParagraph(x));
            var tokenString = Tokenizer.GetTokenArrayFromStrings(preparedParagraphs);
            return HtmlTagger.GetHtmlTaggedStringFromTokenString(tokenString);
        }
    }
}
