namespace Markdown.Tokenization
{
    public interface ITokenReaderConfiguration
    {
        bool IsSeparator(string text, int position);

        int GetSeparatorLength(string text, int position);

        string GetSeparatorValue(string text, int position);
    }
}