using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class TokenExtensions
    {
        public static TagNode ToNode(this Token token) => token.ToTag().ToNode();
        
        public static Tag ToTag(this Token token) => token.Type switch
        {
            TokenType.Bold => Tag.Bold,
            TokenType.Cursive => Tag.Cursive,
            _ => Tag.Text(token.Value)
        };
        
        public static Token ToText(this Token token) => Token.Text(token.Value);
    }

    public static class TagExtensions
    {
        public static Token ToToken(this Tag tag) => tag.Type switch
        {
            TagType.Bold => Token.Bold,
            TagType.Cursive => Token.Cursive,
            _ => Token.Text(tag.Value)
        };
        
        public static TagNode ToNode(this Tag tag) => new(tag);
    }
}