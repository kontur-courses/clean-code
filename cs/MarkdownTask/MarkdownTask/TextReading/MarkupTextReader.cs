namespace MarkdownTask.TextReading
{
    public class MarkupTextReader : IMarkupTextReader
    {
        private int currentPos;
        private string text;

        public MarkupTextReader(string text = default)
        {
            this.text = text;
        }

        public char ReadNextChar()
        {
            var nextChar = currentPos < text.Length - 1 ? text[currentPos] : '\0';
            currentPos++;

            return nextChar;
        }

        public void SetNewText(string text)
        {
            this.text = text;
            currentPos = 0;
        }
    }
}