namespace Markdown.Elements
{
    public abstract class UnderscoreElementTypeBase : EmphasisTypeBase
    {
        public override bool IsIndicatorAt(string markdown, bool[] escapeBitMask, int position)
        {
            if (position + Indicator.Length > markdown.Length)
                return false;

            var underscoreBefore = position != 0 &&
                                   markdown[position - 1] == '_' &&
                                   !escapeBitMask[position - 1];

            var positionAfterIndicator = position + Indicator.Length;

            var underscoreAfter = positionAfterIndicator != markdown.Length &&
                                  markdown[positionAfterIndicator] == '_';

            return !underscoreBefore &&
                   markdown.Substring(position, Indicator.Length) == Indicator &&
                   !escapeBitMask[position] &&
                   !underscoreAfter;
        }
    }
}
