namespace Markdown.Rules
{
    public interface IRules
    {
        bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength);

        bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength,
            string parentSeparator);

        bool IsSeparatorPaired(string separator);

        bool IsSeparatorPairedFor(string firstSeparator, string secondSeparator);

        bool IsSeparatorOpening(string separator);
    }
}