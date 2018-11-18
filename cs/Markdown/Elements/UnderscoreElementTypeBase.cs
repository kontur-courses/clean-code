namespace Markdown.Elements
{
    public abstract class UnderscoreElementTypeBase : EmphasisTypeBase
    {
        public override bool IsIndicatorAt(string markdown, bool[] isEscapedCharAt, int position)
        {
            var positionAfterIndicator = position + Indicator.Length;
            if (positionAfterIndicator > markdown.Length)
                return false;

            var underscoreBefore = markdown.IsCharAt(position - 1, '_') &&
                                   !isEscapedCharAt[position - 1];

            var underscoreAfter = markdown.IsCharAt(positionAfterIndicator, '_');

            return !underscoreBefore &&
                   markdown.Substring(position, Indicator.Length) == Indicator &&
                   !isEscapedCharAt[position] &&
                   !underscoreAfter;
        }
    }
}
