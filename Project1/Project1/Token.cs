namespace Programm
{
    public class Token
    {
        public string RenderedValue;
        public string OriginalValue;
        public bool IsEscapeChar = false;

        public Token(string renderedValue, string originalValue, bool isEscapeChar)
        {
            RenderedValue = renderedValue;
            OriginalValue = originalValue;
            IsEscapeChar = isEscapeChar;
        }
    }
}