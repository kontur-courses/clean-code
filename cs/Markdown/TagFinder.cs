using Markdown.TagHandlers;
using System.Text;

namespace Markdown
{
    public static class TagFinder
    {
        private static readonly HeadingHandler Heading = new();
        private static readonly BoldHandler Bold = new();
        private static readonly ItalicHandler Italic = new();
        private static StringBuilder? htmlText;

        public static Tag? FindTag(StringBuilder markdownText, int i, FindTagSettings settings)
        {
            Tag tag;

            if (char.IsDigit(markdownText[i]))
                return new Tag(htmlText, i);

            if (settings.SearchForHeading && Heading.IsHeadingTagSymbol(markdownText, i))
            {
                tag = Heading.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForBold && Bold.IsBoldTagSymbol(markdownText, i))
            {
                tag = Bold.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForItalic && Italic.IsItalicTagSymbol(markdownText, i))
            {
                tag = Italic.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            return null;
        }
    }
}