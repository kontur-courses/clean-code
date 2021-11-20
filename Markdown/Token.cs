namespace Markdown
{
    public class Token
    {
        private readonly string value;
        public int Length => value.Length;

        public Token(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj) 
            => obj is Token token && Equals(token);

        private bool Equals(Token other) 
            => value == other.value;

        public override int GetHashCode() 
            => value != null ? value.GetHashCode() : 0;

        public override string ToString() 
            => value;
    }
}