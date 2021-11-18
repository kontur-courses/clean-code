namespace MarkdownTask.TextReading
{
    public interface IMarkupTextReader
    {
        char ReadNextChar();

        void SetNewText(string text);
    }
}