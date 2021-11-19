namespace MarkdownTask.TextReading
{
    public interface IMarkupTextReader
    {
        void SetNewText(string text);

        char GetCurrentChar();

        void SkipNextNChars(int skipCharCount);
    }
}