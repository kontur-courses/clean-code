namespace Markdown
{
    public class Tag
    {
        public string Value { get; }
        public Language Language { get; }
        public bool HasWhitespaceBefore { get; }
        public bool HasWhitespaceAfter { get; }

        public Tag(
            string value,
            Language language,
            bool hasWhitespaceBefore,
            bool hasWhitespaceAfter)
        {
            Value = value;
            Language = language;
            HasWhitespaceAfter = hasWhitespaceAfter;
            HasWhitespaceBefore = hasWhitespaceBefore;
        }
    }
}
