namespace MarkDown
{
    public class MdStringReader : IMdReader
    {
        private readonly string line;

        private int _position;
        private int position
        {
            get => _position;
            set => _position = value > line.Length ? line.Length : value;
        }

        public MdStringReader(string line)
        {
            this.line = line;
        }

        public char LookAhead(int offset = 0)
        {
            return line[position + offset];
        }

        public char LookBehind(int offset = 1)
        {
            return line[position - offset];
        }

        public void ShiftPointer(int amountOfChars = 1)
        {
            position += amountOfChars;
        }

        public char[] LookNextChars(int n)
        {
            return line.Substring(position, n).ToCharArray();
        }

        public char[] LookPreviousChars(int n)
        {
            return line.Substring(position - n, n).ToCharArray();
        }

        public bool HasNext()
        {
            return position < line.Length;
        }

        public bool HasNextChars(int n)
        {
            return position + n <= line.Length && position + n > 0;
        }

        public bool HasPrevious()
        {
            return position > 0 && position < line.Length;
        }

        public bool HasPreviousChars(int n)
        {
            return position - n >= 0 && position - n < line.Length;
        }
    }
}