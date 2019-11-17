namespace Markdown
{
    class Token
    {
        public int StartIndex;
        public int Length;
        public string Str;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if ((obj is Token tok) && tok.GetHashCode() == GetHashCode())
                return StartIndex == tok.StartIndex && Length == tok.Length && Str == tok.Str;
            else
                return false;
        }

        public override int GetHashCode() => StartIndex + Length;
    }
}