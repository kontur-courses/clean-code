using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    public class MdItalicStyleFinder : MdEmphasisStyleFinder
    {
        private readonly Style boldStyle;

        public MdItalicStyleFinder(Style mdStyle, string text, Style boldStyle) : base(mdStyle, text)
        {
            this.boldStyle = boldStyle;
        }

        public override bool IsStartTag(int startTagPosition)
        {
            return base.IsStartTag(startTagPosition) && !ContainedInSomeBoldTag(startTagPosition);
        }

        public override bool IsEndTag(int endTagPosition)
        {
            return base.IsEndTag(endTagPosition) && !ContainedInSomeBoldTag(endTagPosition);
        }

        private bool ContainedInSomeBoldTag (int italicTagPosition)
        {
            if (italicTagPosition == 0)
                return Text.IndexOf(boldStyle.StartTag, italicTagPosition) == italicTagPosition;
            var boldTagPosition = Text.IndexOf(boldStyle.StartTag, italicTagPosition - 1);
            return boldTagPosition == italicTagPosition - 1 || boldTagPosition == italicTagPosition;

        }
    }
}
