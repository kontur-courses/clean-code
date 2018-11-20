using Markdown.Analyzers;

namespace Markdown.Elements
{
    public abstract class UnderscoreElementTypeBase : EmphasisTypeBase
    {
        public override bool IsIndicatorAt(SyntaxAnalysisResult syntaxAnalysisResult, int position)
        {
            if (syntaxAnalysisResult.IsInsideWordWithDigitsAt(position) ||
                syntaxAnalysisResult.IsEscapedCharAt(position))
                return false;

            var markdown = syntaxAnalysisResult.Markdown;
            var positionAfterIndicator = position + Indicator.Length;
            if (positionAfterIndicator > markdown.Length)
                return false;

            var underscoreBefore = markdown.IsCharAt(position - 1, '_') &&
                                   !syntaxAnalysisResult.IsEscapedCharAt(position - 1);

            var underscoreAfter = markdown.IsCharAt(positionAfterIndicator, '_');

            return !underscoreBefore &&
                   markdown.Substring(position, Indicator.Length) == Indicator &&
                   !underscoreAfter;
        }
    }
}
