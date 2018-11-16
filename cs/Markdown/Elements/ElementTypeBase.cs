using System;

namespace Markdown.Elements
{
    public abstract class ElementTypeBase : IElementType
    {
        public abstract string Indicator { get; }

        public virtual bool IsOpeningOfElement(string markdown, bool[] escapeBitMask, int position)
        {
            if (escapeBitMask[position])
                return false;

            int positionAfterIndicator = position + Indicator.Length;
            return IsIndicatorAt(markdown, escapeBitMask, position) &&
                   positionAfterIndicator < markdown.Length &&
                   !Char.IsWhiteSpace(markdown[positionAfterIndicator]);
        }

        public virtual bool IsClosingOfElement(string markdown, bool[] escapeBitMask, int position)
        {
            if (escapeBitMask[position])
                return false;

            return IsIndicatorAt(markdown, escapeBitMask, position) &&
                   position >= 1 && !Char.IsWhiteSpace(markdown[position - 1]);
        }

        public abstract bool CanContainElement(IElementType element);
        public abstract bool IsIndicatorAt(string markdown, bool[] escapeBitMask, int position);
    }
}
