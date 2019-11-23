using Markdown.Rules;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders.Tests
{
    public class TestRules : IRules
    {
        public bool IsSeparatorValid(string text, int position, bool isFirst, string separator)
        {
            return position % 2 == 1;
        }

        public bool IsSeparatorValid(string text, int position, bool isFirst, string separator, string parentSeparator)
        {
            return IsSeparatorValid(text, position, isFirst, separator);
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