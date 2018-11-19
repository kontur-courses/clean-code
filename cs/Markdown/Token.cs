namespace Markdown
{
    public class Token
    {
        public readonly string Name;
        public readonly string TextValue, Value;
        public readonly string OpenBracket, CloseBracket;
        public readonly int Priority;

        public Token(string name, string textValue, string value, string openBracket, int priority, string closeBracket = null)
        {
            Name = name;
            TextValue = textValue;
            Value = value;

            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Priority = priority;
        }
    }
}