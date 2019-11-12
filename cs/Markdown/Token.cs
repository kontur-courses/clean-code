namespace Markdown
{
    class Token
    {
        public int StartIndex;
        public int Count;
        public string Str;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if ((obj is Token tok) && tok.GetHashCode() == GetHashCode())
                return StartIndex == tok.StartIndex && Count == tok.Count && Str == tok.Str;
            else
                return false;
        }

        public override int GetHashCode() =>
            StartIndex + Str == null ? 0 : Str.Substring(StartIndex, Count).GetHashCode();
    }
}