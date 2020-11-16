namespace Markdown.Models.Tags
{
    internal class Tag
    {
        public virtual string Opening => string.Empty;
        public virtual string Closing => string.Empty;

        public override bool Equals(object obj)
        {
            if (obj is Tag tag)
                return Opening == tag.Opening && Closing == tag.Closing;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Opening.GetHashCode() * 577) ^ Closing.GetHashCode();
            }
        }
    }
}
