using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Languages;
using Markdown.Tokenizing;

namespace Markdown.Translating
{
    public class Translator
    {
        private readonly Language language;

        public Translator(Language language)
        {
            this.language = language;
        }

        public string Translate(List<Token> tokens)
        {
           var stringBuilder = new StringBuilder();

            foreach (var token in tokens)
                stringBuilder.Append(TranslateToken(token));

            return stringBuilder.ToString();
        }

        private string TranslateToken(Token token)
        {
            if (token.Tag == Tag.Raw)
                return token.Content;

            if (token.IsOpening)
                return language.OpeningTags[token.Tag];

            return language.ClosingTags[token.Tag];
        }
    }
}