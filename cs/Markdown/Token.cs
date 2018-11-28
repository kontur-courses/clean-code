namespace Markdown
{
    public class Token
    {
        public readonly string Value, OpenTag, CloseTag;
        public readonly int Priority, Shift;
        public readonly bool IsParseInside;

        public Token(string value, string openTag, string closeTag, int priority, int shift, bool isParseInside)
        {
            Value = value;
            OpenTag = openTag;
            CloseTag = closeTag;

            Priority = priority;
            Shift = shift;
            IsParseInside = isParseInside;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            var token = other as Token;

            if (token == null)
                return false;

            return token.Value == this.Value
                   && token.OpenTag == this.OpenTag
                   && token.CloseTag == this.CloseTag
                   && token.Priority == this.Priority
                   && token.Shift == this.Shift
                   && token.IsParseInside == this.IsParseInside;
        }
    }
}