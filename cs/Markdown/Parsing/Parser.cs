using System.Collections.Generic;
using System.Text;
using Markdown.Languages;
using Markdown.Tokenizing;

namespace Markdown.Parsing
{
    public class Parser
    {
        private readonly Language language;

        public Parser(Language language)
        {
            this.language = language;
        }

        public string Parse(List<Token> tokens)
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (token.Tag == Tag.Raw)
                    stringBuilder.Append(token.Content);
                else if (token.IsOpening)
                    stringBuilder.Append(language.ConvertOpeningTag(token.Tag));
                else stringBuilder.Append(language.ConvertClosingTag(token.Tag));
            }

            return stringBuilder.ToString();
        }
    }
}
