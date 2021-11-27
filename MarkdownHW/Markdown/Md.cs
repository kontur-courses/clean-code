using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        public static string Render(string markdown)
        {
            var html = "";
            return html;
        }

        internal List<Tag> FindTags(string text, string[] allowedTags)
        {
            return new List<Tag>();
        }

        private string RenderParagraph(List<Tag> tag)
        {
            return "";
        }
    }
}