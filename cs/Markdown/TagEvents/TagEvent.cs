namespace Markdown.TagEvents
{
    public class TagEvent
    {
        public TagSide Side;
        public TagName Name;
        public string Content;

        public TagEvent()
        {
            Side = TagSide.None;
            Name = TagName.Text;
            Content = "";
        }

        public TagEvent(TagSide side, TagName name, string content)
        {
            Side = side;
            Name = name;
            Content = content;
        }

        public override string ToString()
        {
            return $"Content = {Content}\n";
        }

        public bool IsLeftUnderliner()
            => Name == TagName.Underliner && Side == TagSide.Left;

        public bool IsRightUnderliner()
            => Name == TagName.Underliner && Side == TagSide.Right;

        public bool IsLeftDoubleUnderliner()
            => Name == TagName.DoubleUnderliner && Side == TagSide.Left;

        public bool IsRightDoubleUnderliner()
            => Name == TagName.DoubleUnderliner && Side == TagSide.Right;

        public bool IsRightHeader()
            => Name == TagName.Header && Side == TagSide.Right;

        public bool IsNewLineHeader()
            => Name == TagName.NewLine && Side == TagSide.Right;

        public bool IsWordOrNumber()
            => Name == TagName.Word || Name == TagName.Number;

        public bool IsWhiteSpaceOrNewLineOrEof()
            => Name == TagName.Whitespace  || Name == TagName.NewLine
            || Name == TagName.Eof;

        public bool IsUnderliner()
            => Name == TagName.Underliner || Name == TagName.DoubleUnderliner;


        public void ConvertToWord()
        {
            Side = TagSide.None;
            Name = TagName.Word;
        }

        public bool IsHashtagHeader()
            => Side == TagSide.Left && Name == TagName.Header;

        public void ConvertToRightHeader()
        {
            Side = TagSide.Right;
            Name = TagName.Header;
        }

        public bool IsNewLine()
            => Name == TagName.NewLine;

        public bool IsHeader()
            => Name == TagName.Header;

        public bool HasLeftSide()
            => Side == TagSide.Left;

        internal bool HasRightSide()
            => Side == TagSide.Right;

        public bool HasNoSide()
            => Side == TagSide.None;
    }
}
