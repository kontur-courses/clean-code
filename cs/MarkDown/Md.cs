using System;
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

        public string Render(string textParagraph) 
        {
            var text = textParagraph.GetCharStates(GetSymbolsToEscape());
            var parser = new MarkDownParser(text, availableTagTypes);
            var tokens = parser.GetTokens();
            return new ParagraphTag().ToHtml(ProcessText(tokens));
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
                var escapedContent = token.Content == null ? string.Empty : string.Concat(token.Content.Where(s => s.CharState != CharState.Ignored).Select(s => s.Char));
                var tagContent = token.TokenType == TokenType.Tag ? ProcessText(token.InnerTokens) : escapedContent;

                result.Append(token.ToHtml(tagContent));
            }

            return result.ToString();
        }
    }
}
