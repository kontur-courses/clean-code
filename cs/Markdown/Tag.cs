namespace Markdown
{
    public class Tag
    {
        public string Open { get; }
        public string Close { get; }

        public Tag(string open, string close)
        {
            Open = open;
            Close = close;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tag other))
                return false;
            return Equals(other);
        }

        private bool Equals(Tag other)
        {
            return Open == other.Open
                   && Close == other.Close;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Open != null? Open.GetHashCode() : 0) * 397)
                       ^ (Close != null ? Close.GetHashCode() : 0);
            }
        }
    }
}