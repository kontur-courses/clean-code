using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public string TagName { get; }
        private string Mark { get; }
        public int OpenPosition { get; }
        public string Content => GetContent();
        public int Lenght => Content.Length;
        public int ClosePosition { get; }
        private readonly string text;

        private bool Closed => IsClosed();
        private readonly bool Screened;

        public Tag(string tagName, int openPosition, int closePosition, bool isScreened,  string text)
        {
            this.text = text;
            
            TagName = tagName;
            Mark = TagBuilder.marks[tagName];
            
            OpenPosition = openPosition;
            ClosePosition = closePosition;
            
            Screened = isScreened;
        }

        public override string ToString()
        {
            if(!Closed || Screened)
                return $"{Mark}{Content}{Mark}";
            
            return $"<{TagName}>{Content}</{TagName}>";
        }

        private bool IsClosed()
        {
            if (Mark == "#")
                return true;
            
            return text.Substring(OpenPosition, Mark.Length) == text.Substring(ClosePosition - Mark.Length + 1, Mark.Length) && !Content.Any(char.IsDigit);
        }

        private string GetContent()
        {
            var len = Mark != "#" ? ClosePosition - OpenPosition - 2 * Mark.Length + 1 : ClosePosition - OpenPosition;
            if (Screened)
                len--;
            
            return text.Substring(OpenPosition + Mark.Length, len);
        }
    }
}