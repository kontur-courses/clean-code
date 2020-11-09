using System.Collections.Generic;

namespace Markdown
{
    public class TextInfo
    {
        public List<TextInfo> content;
        public string text;
        public string tail;
        private readonly Tag tag;

        public TextInfo(Tag tag = Tag.NoFormatting)
        {
            throw new System.NotImplementedException();
        }

        public void AddContent(TextInfo text)
        {
            throw new System.NotImplementedException();
        }
    }
}