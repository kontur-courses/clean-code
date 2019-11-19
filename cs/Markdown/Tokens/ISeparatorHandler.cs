namespace Markdown.Tokens
{
    public interface ISeparatorHandler
    {
        bool IsSeparator(string text, int position);

        int GetSeparatorLength(string text, int position);

        string GetSeparatorValue(string text, int position);
    }
}