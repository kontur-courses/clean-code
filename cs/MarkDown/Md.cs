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

        public string Render(string textParagraph) {
            var parser = new MarkDownParser(textParagraph, availableTagTypes);
            var tokens = parser.GetTokens();
            var toEscape = GetSymbolsToEscape();
            return new ParagraphTag().ToHtml(ProcessText(tokens).Escape(toEscape));
        }

        private List<string> GetSymbolsToEscape()
        {   
            var result = new List<string>();
            foreach (var tagType in availableTagTypes)
            {
                result.Add(tagType.OpeningSymbol);
                result.Add(tagType.ClosingSymbol);
                if (tagType.Parameter == null) continue;
                result.Add(tagType.Parameter.OpeningSymbol);
                result.Add(tagType.Parameter.ClosingSymbol);
            }
            result.Add(@"\");
            return result;
        }

        private string ProcessText(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                var tagContent = token.TokenType == TokenType.Tag ? ProcessText(token.InnerTokens) : token.Content;

                result.Append(token.ToHtml(tagContent));
            }

            return result.ToString();
        }
    }
}
