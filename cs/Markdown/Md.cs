using System.Text;
using Markdown.TagHandlers;

namespace Markdown
{
    public class Md
    {
        private readonly FindTagSettings settings = new(true, true, true);

        public string Render(string markdownText)
        {
            var htmlText = new StringBuilder(markdownText);

            for (var i = 0; i < htmlText!.Length; i++)
            {
                var tag = TagFinder.FindTag(htmlText, i, settings);

                if (tag == null || tag.Text == null)
                    continue;

                htmlText = tag.Text;
                i = tag.Index;
            }

            return htmlText.ToString();
        }
    }
}