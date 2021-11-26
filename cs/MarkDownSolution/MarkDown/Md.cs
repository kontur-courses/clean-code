using System;
using System.Collections.Generic;

namespace MarkDown
{
    public static class Md
    {
        public static string Render(string input)
        {
            var paragraphList = new List<string>();
            var paragraphs = TextPreparer.PrepareText(input);
            foreach (var paragraph in paragraphs)
            {
                var token = Tokenizer.GetToken(paragraph);
                var htmlParagraph = HtmlTagger.GetString(token, paragraph);
                paragraphList.Add(htmlParagraph);
            }
            return string.Join(Environment.NewLine, paragraphList);
        }
    }
}
