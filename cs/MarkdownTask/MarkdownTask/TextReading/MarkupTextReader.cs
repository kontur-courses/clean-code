namespace MarkdownTask.TextReading
{
    public class MarkupTextReader : IMarkupTextReader
    {
        private int currentPos;
        private string text;

        public void SetNewText(string textToRead)
        {
            text = textToRead;
        }

        public char GetCurrentChar()
        {
            return currentPos < text.Length ? text[currentPos] : '\0';
        }

        public void SkipNextNChars(int skipCharCount)
        {
            currentPos += skipCharCount;
        }
    }
}