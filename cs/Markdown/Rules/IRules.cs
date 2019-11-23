namespace Markdown.Rules
{
    public interface IRules
    {
        bool IsSeparatorValid(string text, int position, bool isFirst, string separator);

        bool IsSeparatorValid(string text, int position, bool isFirst, string separator,
            string parentSeparator);

        bool IsSeparatorPaired(string separator);

        bool IsSeparatorPairedFor(string firstSeparator, string secondSeparator);

        bool IsSeparatorOpening(string separator);
    }
}