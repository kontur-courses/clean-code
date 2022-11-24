using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal static class TokenToHtmlParser
    {
        private const string openTagFormat = "<{0}>";
        private const string closeTagFormat = "</{0}>";

        private static readonly Dictionary<TokenObjectType, string> objectTagBodies = new Dictionary<TokenObjectType, string>()
        {
            { TokenObjectType.Italic, "em" },
            { TokenObjectType.Strong, "strong" },
            { TokenObjectType.Header, "h1" },
        };

        public static string GetHtmlTextFromTokens(List<Token> tokens)
        {
            var htmlText = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token is ObjectOpenToken)
                {
                    htmlText.AppendFormat(openTagFormat, objectTagBodies[(token as ObjectOpenToken)!.ObjectType]);
                    continue;
                }

                if (token is ObjectCloseToken)
                {
                    htmlText.AppendFormat(closeTagFormat, objectTagBodies[(token as ObjectCloseToken)!.ObjectType]);
                    continue;
                }

                htmlText.Append(token.Text);
            }

            return htmlText.ToString();
        }
    }
}