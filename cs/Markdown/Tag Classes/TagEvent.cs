using System;
using System.Runtime.Remoting.Messaging;

namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public Side Side;
        public Mark Mark;
        public string TagContent;

        public TagEvent(){ }

        public TagEvent(Side side, Mark mark, string content)
        {
            Side = side;
            Mark = mark;
            TagContent = content;
        }

        public override string ToString()
        {
            return $"TagContent = {TagContent}\n";
        }

        public bool IsUnderliner()
            => Mark == Mark.Underliner || Mark == Mark.DoubleUnderliner;

        public bool IsTextContainingWhitespace()
            => Mark == Mark.Text && TagContent.Contains(" ");

        public void ChangeMarkAndSideTo(Mark mark, Side side)
        {
            Mark = mark;
            Side = side;
        }

        public void ChangeSideTo(Side side)
            => Side = side;

        public void ChangeMarkTo(Mark newMark)
            => Mark = newMark;

        public bool IsPlainText()
            => Mark == Mark.Text;

        public bool IsEmpty()
            => string.IsNullOrEmpty(this.TagContent);

        public bool IsSideUnknown()
            => Side == Side.Unknown;
    }
}
