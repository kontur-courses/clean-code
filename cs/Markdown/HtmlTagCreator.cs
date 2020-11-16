using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlTagCreator
    {
        public string CreateBold(string text)
        {
            return $@"<strong>{text}</strong>";
        }
        public string CreateItalics(string text)
        {
            return $@"<em>{text}</em>";
        }

        public string CreateHeading(string text)
        {
            return $@"<h1>{text}</h1>";
        }

        public string CreateLink(string text, string link)
        {
            return $"<a href=\"{link}\">{text}</a>";
        }
    }
}
