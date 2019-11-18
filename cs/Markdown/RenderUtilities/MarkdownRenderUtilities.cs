using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public static class MarkdownRenderUtilities
    {
        public static Renderer GetMarkdownRenderer()
        {
            return new Renderer(new List<ITokenHandler>()
            {
                 new SimpleHandler(GetSimpleTokenHandleDescriptions()),
                 new PairedHandler(GetPairedTokenHandleDescriptions())
            });
        }

        public static List<SimpleTokenHandleDescription> GetSimpleTokenHandleDescriptions()
        {
            var handleDescriptions = new List<SimpleTokenHandleDescription>()
            {
                new SimpleTokenHandleDescription(TokenType.Text, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.WhiteSpace, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.Digits, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.Escape, (token) => token.Text.Substring(1))
            };

            return handleDescriptions;
        }

        public static List<PairedTokenHandleDescription> GetPairedTokenHandleDescriptions()
        {
            bool isOpening(TokenType tokenType, List<Token> tokenList, int i)
            {
                if (tokenList[i].TokenType != tokenType || i + 1 == tokenList.Count
                || tokenList[i + 1].TokenType == TokenType.WhiteSpace)
                    return false;
                if (i != 0 && tokenList[i + 1].TokenType == TokenType.Digits
                && tokenList[i - 1].TokenType == TokenType.Digits)
                    return false;

                return true;
            }
            bool isClosing(TokenType tokenType, List<Token> tokenList, int i)
            {
                if (tokenList[i].TokenType != tokenType || i == 0
                || tokenList[i - 1].TokenType == TokenType.WhiteSpace)
                    return false;
                if (i + 1 != tokenList.Count && tokenList[i + 1].TokenType == TokenType.Digits
                && tokenList[i - 1].TokenType == TokenType.Digits)
                    return false;

                return true;
            }
            string getTokenString(string tag, Token token, bool hasPair, bool isClosed)
            {
                if (!hasPair)
                    return token.Text;
                if (isClosed)
                    return $"</{tag}>";
                return $"<{tag}>";
            }

            var handleDescriptions = new List<PairedTokenHandleDescription>()
            {
                new PairedTokenHandleDescription(TokenType.Emphasis, 
                    (tokenList, i) => isOpening(TokenType.Emphasis, tokenList, i),
                    (tokenList, i) => isClosing(TokenType.Emphasis, tokenList, i),
                    (token, hasPair, isClosed) => getTokenString("em", token, hasPair, isClosed)
                ),
                new PairedTokenHandleDescription(TokenType.Strong,
                    (tokenList, i) => isOpening(TokenType.Strong, tokenList, i),
                    (tokenList, i) => isClosing(TokenType.Strong, tokenList, i),
                    (token, hasPair, isClosed) => getTokenString("strong", token, hasPair, isClosed)
                ),
            };

            return handleDescriptions;
        }
    }
}
