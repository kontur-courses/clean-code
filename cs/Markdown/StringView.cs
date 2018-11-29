namespace Markdown
{
    public class StringView
    {
        public string SourceString { get; }
        public int Position { get; set; }

        public StringView(string sourceString, int position)
        {
            SourceString = sourceString;
            Position = position;
        }

        public char this[int i]
        {
            get
            {
                var index = i + Position;
                if (index < 0 || index >= SourceString.Length)
                    return '\0';
                return SourceString[i + Position];
            }
        }
    }
}
