namespace Markdown
{
    public class TokenType
    {
        public TokenType(string name, string template, string htmlTag, TokenLocationType tokenLocationType)
        {
            Name = name;
            Template = template;
            HtmlTag = htmlTag;
            TokenLocationType = tokenLocationType;
        }

        public string Name { get; }
        public string Template { get; }
        public string HtmlTag { get; }
        public TokenLocationType TokenLocationType { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}