using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Translator
    {
        public static string Translate(List<PartText> partsText)
        {
            var htmlStr = new StringBuilder();
            
            htmlStr.Append("<p>");
            
            foreach (var partText in partsText)
            {
                if (partText.Tag is null)
                    htmlStr.Append(partText.Text);
                else if (partText.Tag.Status==TagStatus.NoOpen)
                    htmlStr.Append(partText.Tag.TagInfo.Symbol);
                else if (partText.Tag.Status == TagStatus.Open)
                    htmlStr.Append(partText.Tag.TagInfo.SymbolOpen);
                else if (partText.Tag.Status == TagStatus.Close)
                    htmlStr.Append(partText.Tag.TagInfo.SymbolClose);
            }

            htmlStr.Append("</p>");

            return htmlStr.ToString();
        }
    }
}
