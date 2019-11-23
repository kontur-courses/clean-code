using Markdown.Rules;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders.Tests
{
    public class TestRules : IRules
    {
        public bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength)
        {
            return position % 2 == 1;
        }

        public bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength, string parentSeparator)
        {
            return IsSeparatorValid(text, position, isFirst, separatorLength);
        }

        public bool IsSeparatorPaired(string separator)
        {
            return true;
        }

        public bool IsSeparatorPairedFor(string firstSeparator, string secondSeparator)
        {
            return true;
        }

        public bool IsSeparatorOpening(string separator)
        {
            return true;
        }
    }
}