using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void VisitText(TextToken textToken, StringBuilder stringBuilder)
        {
            stringBuilder.Append(textToken.Text);
        }

        public void VisitTag(TagToken tagToken, StringBuilder stringBuilder)
        {
            var (openTag, closingTag) = translateDictionary[tagToken.MdTag];
            stringBuilder.Append(openTag);
            foreach (var children in tagToken.ChildrenTokens)
            {
                children.Translate(this, stringBuilder);
            }

            stringBuilder.Append(closingTag);
        }
    }
}