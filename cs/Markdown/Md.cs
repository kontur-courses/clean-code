using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly Heading heading = new();
        private readonly Bold bold = new();
        private readonly Italic italic = new();

        public string Render(string markdownText)
        {
            var htmlText = new StringBuilder();

            for (var i = 0; i < markdownText.Length; i++)
            {
                if (markdownText[i] == '#')
                {
                    var tag = heading.GetHtmlTag(markdownText, i);
                    htmlText = tag.Item1;
                    i = tag.Item2;
                    continue;
                }

                if (i + 1 < markdownText.Length &&
                    markdownText[i] == '_' && markdownText[i + 1] == '_')
                {
                    var tag = bold.GetHtmlTag(markdownText, i);
                    htmlText = tag.Item1;
                    i = tag.Item2;
                    continue;
                }

                if (markdownText[i] == '_')
                {
                    var tag = italic.GetHtmlTag(markdownText, i);
                    htmlText = tag.Item1;
                    i = tag.Item2;
                }
            }

            return htmlText.ToString();
        }
    }
}