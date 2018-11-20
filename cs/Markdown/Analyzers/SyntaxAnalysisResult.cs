namespace Markdown.Analyzers
{
    public class SyntaxAnalysisResult
    {
        public string Markdown { get; }
        private bool[] isEscapedCharAt;
        private bool[] isInsideWordWithDigitsAt;

        public SyntaxAnalysisResult(string markdown, bool[] isEscapedCharAt, bool[] isInsideWordWithDigitsAt)
        {
            Markdown = markdown;
            this.isEscapedCharAt = isEscapedCharAt;
            this.isInsideWordWithDigitsAt = isInsideWordWithDigitsAt;
        }

        public bool IsEscapedCharAt(int position)
        {
            return isEscapedCharAt[position];
        }

        public bool IsInsideWordWithDigitsAt(int position)
        {
            return isInsideWordWithDigitsAt[position];
        }
    }
}
