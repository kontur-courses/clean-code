using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class HtmlCreator
    {
        private readonly Dictionary<string, (string opening, string closing)> fullTagToRenderMapping =
            new Dictionary<string, (string opening, string closing)>
            {
                ["_"] = ("<em>", "</em>"), ["__"] = ("<strong>", "</strong>")
            };

        public string CreateFromTokens(IEnumerable<Token> tokens) => string.Join("", tokens.Select(RenderToken));

        private string RenderToken(Token token)
        {
            switch (token)
            {
                case StringToken stringToken:
                    return stringToken.Value;
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
                            $"{text.Substring(0, position)}{renderedToken}{text.Substring(position+ innerToken.Length)}";
                    }

                    return
                        $"{opening}{text.Substring(tagToken.FullTag.Length, text.Length - 2 * tagToken.FullTag.Length)}{closing}";
                }
                default:
                    return "";
            }
        }
    }
}
