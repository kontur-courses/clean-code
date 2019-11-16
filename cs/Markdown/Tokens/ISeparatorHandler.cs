namespace Markdown.Tokens
{
    public interface ISeparatorHandler
    {
        bool IsSeparator(string text, int position);

        int GetSeparatorLength(string text, int position);

        bool IsSeparatorValid(string text, int position, bool isFirst);
    }
}