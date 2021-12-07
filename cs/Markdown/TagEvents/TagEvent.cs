using System;
using System.Runtime.Remoting.Messaging;

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

        public static TagEvent GetHeaderTagEvent(char symbol)
        {
            return symbol == '#'
                ? new TagEvent(TagSide.Left, TagName.Header, symbol.ToString())
                : new TagEvent(TagSide.Right, TagName.Header, symbol.ToString());
        }
        public static TagEvent GetTextTagEvent(string content)
            => new TagEvent(TagSide.None, TagName.Text, content);

        public override string ToString()
        {
            return $"Content = {Content}\n";
        }

        public bool IsWordOrNumber()
            => Name == TagName.Word || Name == TagName.Number;

        public bool IsWhiteSpaceOrNewLineOrEof()
            => Name == TagName.Whitespace  || Name == TagName.NewLine
            || Name == TagName.Eof;

        public bool IsUnderliner()
            => Name == TagName.Underliner || Name == TagName.DoubleUnderliner;

        public bool IsTextContainingWhitespace()
            => Name == TagName.Text && Content.Contains(" ");

        public void ChangeMarkAndSideTo(TagName mark, TagSide side)
        {
            Name = mark;
            Side = side;
        }

        public void ChangeSideTo(TagSide side)
            => Side = side;

        public void ChangeMarkTo(TagName newMark)
            => Name = newMark;

        public bool IsPlainText()
            => Name == TagName.Text;

        public bool IsEmpty()
            => string.IsNullOrEmpty(this.Content);

        public void ConvertToWord()
        {
            Side = TagSide.None;
            Name = TagName.Word;
        }

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
