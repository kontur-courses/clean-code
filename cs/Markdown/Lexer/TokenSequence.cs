namespace Markdown.Lexer
{
    internal class TokenSequence : ISequence<Token>
    {
        private readonly ISequence<char> input;

        public int Location => input.Location;
        public bool End => input.End;

        internal TokenSequence(ISequence<char> input)
        {
            this.input = input;
        }

        public Token GetNext()
        {
            throw new System.NotImplementedException();
        }
        
        public Token Peek()
        {
            throw new System.NotImplementedException();
        }
    }
}