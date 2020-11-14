using System.Collections.Generic;

namespace Markdown
{
    public class TextInfo
    {
        public readonly List<TextInfo> Content;
        public string Text { get; private set; }
        public Tag Tag { get; private set; }

        public TextInfo(Tag tag = Tag.NoFormatting, string text = null)
        {
            Tag = tag;
            Text = Escape(text);
            Content = new List<TextInfo>();
        }

        public void AddText(string text)
        {
            if (Content.Count != 0 || Text != null)
                Content.Add(new TextInfo(text:text));
            else
                Text = Escape(text);
        }

        public void AddContent(TextInfo textInfo)
        {
            Content.Add(textInfo);
        }

        public void ToNoFormatting()
        {
            var underscores = Tag == Tag.Bold ? "__" : "_";
            Text = underscores + Text;
            Tag = Tag.NoFormatting;
        }
        
        private static string Escape(string text)
        {
            return text?.Replace("\\\\", "\\")
                .Replace("\\_", "_")
                .Replace("\\#", "#");
        }
    }
}