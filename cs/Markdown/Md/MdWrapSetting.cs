namespace Markdown
{
    public enum MdWrapType
    {
        Text = 1,
        Paragraph = 2
    }

    public class MdWrapSetting
    {
        public string MdTag { get; }
        public string HtmlOpenTag { get; }
        public string HtmlCloseTag => HtmlOpenTag.StartsWith('<') ? HtmlOpenTag.Insert(1, @"/") : HtmlOpenTag;
        public MdWrapType WrapType { get; }

        public MdWrapSetting(string mdTag, string htmlOpenTag, MdWrapType wrapType)
        {
            MdTag = mdTag;
            HtmlOpenTag = htmlOpenTag;
            WrapType = wrapType;
        }
    }
}