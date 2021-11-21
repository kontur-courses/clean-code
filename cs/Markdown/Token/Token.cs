namespace Markdown
{
    public class Token
    {
        public string Value { get; }
        public int Position { get; }
        public MdWrapSetting WrapSetting { get; }

        public Token(string value, int position, MdWrapSetting wrapSetting)
        {
            Value = value;
            Position = position;
            WrapSetting = wrapSetting;
        }

        public virtual string Render()
        {
            return Wrap(Value);
        }

        protected string Wrap(string value)
        {
            return WrapSetting.HtmlOpenTag + value.Substring(WrapSetting.MdTag.Length,
                       value.Length - WrapSetting.MdTag.Length *
                       (WrapSetting.WrapType == MdWrapType.Paragraph ? 1 : 2)) +
                   WrapSetting.HtmlCloseTag;
        }
    }
}