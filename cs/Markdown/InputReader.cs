namespace Markdown
{
    public class InputReader
    {
        private readonly string Text;
        public int CurrentPosition { get; private set; }
        private readonly FixedSizeQueue<char> LastNchars;
        public InputReader(string text)
        {
            Text = text;
            CurrentPosition = 0;
            LastNchars = new FixedSizeQueue<char>(3);
        }

        public char Current()
        {
            return CurrentPosition>=Text.Length ? default : Text[CurrentPosition];
        }

        public char Previous()
        {
            return CurrentPosition == 0 ? default : Text[CurrentPosition - 1];
        }

        public char PeekNext()
        {
            return CurrentPosition == Text.Length-1 ? default : Text[CurrentPosition + 1];
        }

        public void Next()
        {
            CurrentPosition++;
        }
    }
}
