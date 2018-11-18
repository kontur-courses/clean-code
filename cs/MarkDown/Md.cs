using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class Md
    {
        private readonly List<TagType> availableTagTypes;

        public Md(IEnumerable<TagType> availableTagTypes)
        {
            this.availableTagTypes = availableTagTypes.ToList();
        }

        public string Render(string textParagraph) => 
            new ParagraphTag().ToHtml(ProcessText(textParagraph, availableTagTypes))
                .Escape(availableTagTypes.Select(t => t.SpecialSymbol));

        private string ProcessText(string text, IEnumerable<TagType> tagTypes)
        {
            var textStream = new TextStream(text);
            var parser = new MarkDownParser(textStream, tagTypes);
            var tokens = parser.GetTokens();
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                string htmlTag;
                if (token.TokenType == TokenType.Tag)
                {
                    var innerTagTypes = availableTagTypes.Where(e => token.TagType.IsInAvailableNestedTagTypes(e)).ToList();
                    var tokenContent = innerTagTypes.Any() ? ProcessText(token.Content, innerTagTypes) : token.Content;
                    htmlTag = token.TagType.ToHtml(tokenContent);
                }
                else
                    htmlTag = token.Content;

                result.Append(htmlTag);
            }

            return result.ToString();
        }
    }
}
