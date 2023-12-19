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
                return new Tag(markdownText, i);

            if (Screening(markdownText, i) && Screening(markdownText, i - 1))
            {
                markdownText.Remove(i - 2, 2);
                i -= 2;
            }
            
            if (settings.SearchForHeading && Heading.IsHeadingTagSymbol(markdownText, i))
            {
                if (Screening(markdownText, i))
                    return new Tag(markdownText.Remove(i - 1, 1), i);
                
                tag = Heading.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForBold && Bold.IsBoldTagSymbol(markdownText, i))
            {
                if (Screening(markdownText, i))
                    return new Tag(markdownText.Remove(i - 1, 1), i);

                tag = Bold.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            if (settings.SearchForItalic && Italic.IsItalicTagSymbol(markdownText, i))
            {
                if (Screening(markdownText, i))
                    return new Tag(markdownText.Remove(i - 1, 1), i);

                tag = Italic.GetHtmlTag(markdownText, i);
                htmlText = tag.Text;
                i = tag.Index;
                return new Tag(htmlText, i);
            }

            return null;
        }

        private static bool Screening(StringBuilder markdownText, int i) =>
            i - 1 >= 0 && markdownText[i - 1] == '\\';  
    }
}