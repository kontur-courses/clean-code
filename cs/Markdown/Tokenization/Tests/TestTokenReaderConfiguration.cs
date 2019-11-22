namespace Markdown.Tokenization.Tests
{
    public class TestTokenReaderConfiguration : ITokenReaderConfiguration
    {
        public bool IsSeparator(string text, int position)
        {
            return char.IsWhiteSpace(text[position]);
        }

        public int GetSeparatorLength(string text, int position)
        {
            return 1;
        }

        public string GetSeparatorValue(string text, int position)
        {
            return " ";
        }
    }
}