using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HTMLRenderer
    {
        //Превращает оставшиеся корректные токены в HTML-теги
        public string Render(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                switch (token)
                {
                    case PairToken pairToken:
                        result.Append(pairToken.IsFirst
                            ? TokenTypesTranslator.GetHtmlTagFromTokenType(pairToken.Type).First
                            : TokenTypesTranslator.GetHtmlTagFromTokenType(pairToken.Type).Second);
                        break;
                    case HeaderToken headerToken:
                        result.Append(TokenTypesTranslator.GetHtmlTagFromTokenType(headerToken.Type).First);
                        break;
                    default:
                        result.Append(token.Content);
                        break;
                }
            }

            return result.ToString();
        }
    }
}
