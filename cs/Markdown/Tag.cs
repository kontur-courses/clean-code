namespace Markdown
{
    class Tag
    {
        public string Id { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Tag t && GetHashCode() == t.GetHashCode())
                return t.Id == Id && t.Value == Value;
            else
                return false;
        }

        public override int GetHashCode() => 
            Id == null ? 0 : Id.GetHashCode() + Value == null ? 0 : Value.GetHashCode();
    }
}