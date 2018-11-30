namespace Markdown
{
    using System.Collections.Generic;
    using System.Linq;

    internal class HtmlCreator
    {
        private readonly Dictionary<string, (string opening, string closing)> fullTagToRenderMapping;

        public HtmlCreator(Dictionary<string, (string opening, string closing)> tagTypeDictionary)
        {
            fullTagToRenderMapping = tagTypeDictionary;
        }

        public string CreateFromTokens(IEnumerable<Token> tokens) =>
            string.Join(string.Empty, tokens.Select(RenderToken));

        private string RenderToken(Token token)
        {
            switch (token)
            {
                case StringToken _:
                    return token.Value;
                case PairedTagToken tagToken:
                    {
                        var (opening, closing) = fullTagToRenderMapping[tagToken.FullTag];
                        if (tagToken.InnerTokens == null)
                            return $"{opening}{tagToken.Content}{closing}";
                        var text = tagToken.Value;
                        foreach (var innerToken in tagToken.InnerTokens)
                        {
                            var renderedToken = RenderToken(innerToken);
                            var position = innerToken.Position - token.Position;
                            text =
                                $"{text.Substring(0, position)}{renderedToken}{text.Substring(position + innerToken.Length)}";
                        }

                        var tagLength = tagToken.FullTag.Length;
                        return $"{opening}{text.Substring(tagLength, text.Length - 2 * tagLength)}{closing}";
                    }

                default:
                    return string.Empty;
            }
        }
    }
}
