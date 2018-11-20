namespace Markdown.Analyzers
{
    public class SyntaxAnalyzer
    {
        public static SyntaxAnalysisResult AnalyzeSyntax(string markdown)
        {
            var isEscapeCharAt = EscapesAnalyzer.GetBitMaskOfEscapedChars(markdown);
            var isInsideWordWithDigitsAt = WordsContainingDigitsAnalyzer.Analyze(markdown);

            return new SyntaxAnalysisResult(markdown, isEscapeCharAt, isInsideWordWithDigitsAt);
        }
    }
}
