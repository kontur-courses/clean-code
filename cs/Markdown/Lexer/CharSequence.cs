namespace Markdown.Lexer
{
    internal class CharSequence : ISequence<char>
    {
        private readonly string input;
        
        public int Location { get; private set; }
        public bool End => Location >= input.Length;

        internal CharSequence(string input)
        {
            this.input = input;
        }
        
        public char GetNext() => input[Location++];

        public char Peek() => input[Location];
    }
}