namespace Markdown
{
    public class Tag
    {
        public string Value { get; }
        public Language Language { get; }

        public Tag(string value, Language language)
        {
            Value = value;
            Language = language;
        }

        public override bool Equals(object obj)
        {
            var otherTag = (Tag) obj;
            if (otherTag == null)
                return false;
            return Value.Equals(otherTag.Value) 
                   && Language.Equals(otherTag.Language);
        }
    }
}
