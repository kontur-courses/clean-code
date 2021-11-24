using Markdown.Tokens;

namespace Markdown.Extensions
{
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