using System.Linq;

namespace Markdown
{
    public class Tag
    {
        public bool Closed;
        private readonly bool Screened;

        public Tag(string tagName, bool isScreened)
        {
            TagName = tagName;
            Screened = isScreened;
            Mark = TagBuilder.GetMarkByHtmlTag(tagName);
        }

        public string TagName { get; }
        private string Mark { get; }
        public int OpenPosition { get; set; }
        public int ClosePosition { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            if (!Closed || Screened)
                return $"{Mark}{Content}{Mark}";

            return $"<{TagName}>{Content}</{TagName}>";
        }
    }
}