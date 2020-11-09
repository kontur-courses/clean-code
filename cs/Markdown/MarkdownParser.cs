using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : IParser
    {
        private TextInfo textInfo;
        private Stack<Tag> tagHierarchy;

        public MarkdownParser()
        {
            textInfo = new TextInfo();
            tagHierarchy = new Stack<Tag>();
            tagHierarchy.Push(Tag.NoFormatting);
        }

        public TextInfo ParseText(string text)
        {
            throw new System.NotImplementedException();
        }

        private void ParseBoldTag(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void ParseItalicTag(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void ParseHeadingTag(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void ParseTextInTag(char symbol)
        {
            throw new System.NotImplementedException();
        }
    }
}