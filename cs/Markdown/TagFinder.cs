using System.Reflection.Metadata.Ecma335;
using Markdown.Tags;
using System.Text;

namespace Markdown
{
    public static class TagFinder
    {
        private static readonly HeadingHandler heading = new();
        private static readonly BoldHandler bold = new();
        private static readonly ItalicHandler italic = new();
        private static StringBuilder htmlText;

        public static Tag? FindTag(string markdownText, int i, FindTagSettings settings)
        {
            Tag tag;

            if (settings.SearchForHeading && heading.IsHeadingTagSymbol(markdownText, i))
            {
                tag = heading.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForBold && bold.IsBoldTagSymbol(markdownText, i))
            {
                tag = bold.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForItalic && italic.IsItalicTagSymbol(markdownText, i))
            {
                tag = italic.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            return null;
        }
    }
}
