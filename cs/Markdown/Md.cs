using System.Text;
using Markdown.TagHandlers;

namespace Markdown
{
    public class Md
    {
        private readonly FindTagSettings settings = new(true, true, true);

        private TagFinder tagFinder = new(new List<IHtmlTagCreator>
        {
            new BoldHandler(), new HeadingHandler(), new ItalicHandler()
        });

        public string Render(string markdownText)
        {
            var htmlText = new StringBuilder(markdownText);

            for (var i = 0; i < htmlText.Length; i++)
            {
                var tag = tagFinder.FindTag(htmlText, i, settings, null);

                if (tag?.Text == null)
                    continue;

                htmlText = tag.Text;
                i = tag.Index;
            }

            return htmlText.ToString();
        }
    }
}