namespace Markdown.Analyzers
{
    public class SyntaxAnalyzer
    {
        public static SyntaxAnalysisResult AnalyzeSyntax(string markdown)
        {
            var isEscapeCharAt = EscapesAnalyzer.GetBitMaskOfEscapedChars(markdown);
            var isInsideWordWithDigitsAt = IncludingToWordWithDigitsAnalyzer.GetMarkers(markdown);

            return new SyntaxAnalysisResult(markdown, isEscapeCharAt, isInsideWordWithDigitsAt);
        }
    }
}
