using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        private FindTagSettings settings = new(true, true, true);
        public string Render(string markdownText)
        {
            for (var i = 0; i < markdownText.Length; i++)
            {
                var tag1 = TagFinder.FindTag(markdownText, i, settings);

                if (tag1 == null)
                    continue;

                markdownText = tag1.Text.ToString();
                i = tag1.Index;
            }

            return markdownText;
        }
    }
}