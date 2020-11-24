using Markdown.Tokens;

namespace Markdown.Converters
{
    public class TagConvertor : ITokenConverter
    {
        private readonly IConverter converter;
        private readonly string tag;

        public TagConvertor(IConverter converter, string tag)
        {
            this.converter = converter;
            this.tag = tag;
        }

        public string Convert(Token token)
        {
            return token.ChildTokens.Count != 0
                ? $"<{tag}>{converter.ConvertTokens(token.ChildTokens)}</{tag}>"
                : $"<{tag}>{token.Value}</{tag}>";
        }
    }
}