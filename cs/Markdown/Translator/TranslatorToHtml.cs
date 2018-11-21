using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public class TranslatorToHtml : ITranslator
    {
        private static readonly Dictionary<string, (string, string)> translateDictionary =
            new Dictionary<string, (string, string)>
            {
                {"_", ("<em>", "</em>")},
                {"__", ("<strong>", "</strong>")},
                {"`", ("<code>", "</code>")}
            };

        public string VisitTag(TagToken tagToken)
        {
            var (openTag, closingTag) = translateDictionary[tagToken.MdTag];
            
            return string.Concat(tagToken.TokensInnerTag.Aggregate(openTag, (current, token) => current + token.Translate(this)),
                closingTag);
        }

        public string VisitText(TextToken textToken)
        {
            return textToken.Text;
        }
    }
}