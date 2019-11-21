using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HTMLRenderer
    {
        //Превращает оставшиеся корректные токены в HTML-теги
        public string Render(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            var tokenNumber = 0;
            HeaderToken currentHeaderToken = null;
            var lastTokenNumber = tokens.Count() - 1;
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
                        currentHeaderToken = headerToken;
                        break;
                    default:
                        result.Append(token.Content);
                        break;
                }

                if (currentHeaderToken != null && tokenNumber == lastTokenNumber)
                {
                    result.Append(TokenTypesTranslator.GetHtmlTagFromTokenType(currentHeaderToken.Type).Second);
                }

                tokenNumber++;
            }

            if (currentHeaderToken == null)
            {
                result.Append("<br><br>");
            }

            return result.ToString();
        }
    }
}