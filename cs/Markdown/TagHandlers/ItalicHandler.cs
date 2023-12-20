using System.Text;

namespace Markdown.TagHandlers
{
    public class ItalicHandler : IHtmlTagCreator
    {
        private const char Italic = '_';
        private const int MdTagLength = 2;
        private const int HtmlTagLength = 9;

        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex, string? parentClosingTag)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1, parentClosingTag);

            if (closingTagIndex == -1)
                return null;

            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);
            var newHtmlTagLength = closingTagIndex + (HtmlTagLength - MdTagLength);

            return new Tag(htmlTag, newHtmlTagLength);
        }

        internal bool IsItalicTagSymbol(StringBuilder markdownText, int i)
        {
            if (i > 0)
                return
                    (markdownText[i] == Italic && i + 1 >= markdownText.Length) ||
                    (markdownText[i] == Italic && markdownText[i - 1] != Italic && markdownText[i + 1] != Italic);

            return markdownText[i] == Italic;
        }

        private StringBuilder? CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingTagIndex)
        {
            if (openTagIndex + 1 == closingTagIndex)
                return markdownText;

            markdownText.Remove(closingTagIndex, 1);
            markdownText.Insert(closingTagIndex, "</em>");
            markdownText.Remove(openTagIndex, 1);
            markdownText.Insert(openTagIndex, "<em>");

            return markdownText;
        }

        private int FindClosingTagIndex(StringBuilder markdownText, int openTagIndex, string parentClosingTag)
        {
            var oneWord = true;
            BoldHandler bold = new BoldHandler();

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == ' ')
                    oneWord = false;
    
                if (char.IsDigit(markdownText[i]))
                    return i;

                if (IsItalicTagSymbol(markdownText, i) && TagSettings.IsCorrectClosingSymbol(markdownText, i, Italic))
                {
                    if (!oneWord && TagSettings.IsHalfOfWord(markdownText, i)) 
                        return -1;
                    
                    return i;
                }

                if (bold.IsBoldTagSymbol(markdownText, i))
                    if (parentClosingTag == "__")
                        return -1;
                    else
                    {
                        var newTag = bold.GetHtmlTag(markdownText, i, "_");
                        i = newTag.Index;                   
                    }
            }

            return -1;
        }
    }
}