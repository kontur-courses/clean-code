using Markdown.Markdown;

namespace Markdown.Tokens
{
    public static class TokenFilter
    {
        private static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static List<Token> Filter(this List<Token> tokens, string MarkdownString)
        {
            if (tokens.Count == 0)
                throw new ArgumentException("Empty tokens must not been filtered");
            return tokens
                .RemoveFields()
                .RemoveDigits(MarkdownString)
                .RemoveSingleTokens(MarkdownString)
                .RemoveStrongInItalics()
                .ToList();
        }

        private static IEnumerable<Token> RemoveFields(this List<Token> tokens)
        {
            var count = tokens.Count;
            for (var i = 0; i < count; i++)
            {

                if (tokens[i].Type != TokenType.Field)
                {
                    yield return tokens[i];
                    continue;
                }
                tokens[i].SetToDefault();
                if (i + 1 < count && tokens[i + 1].Type != TokenType.Default)
                {
                    tokens[i].SetToUnsee();
                    tokens[i + 1].SetToDefault();
                }
                yield return tokens[i];
            }
        }

        private static IEnumerable<Token> RemoveDigits(this IEnumerable<Token> tokens, string mdString)
        {
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default)
                {
                    yield return token;
                    continue;
                }

                if (!HaveDigit(mdString, token))
                {
                    yield return token;
                    continue;
                }

                token.SetToDefault();
                yield return token;
            }
        }

        private static IEnumerable<Token> RemoveStrongInItalics(this IEnumerable<Token> tokens)
        {
            var lastOpenIsItalics = false;

            foreach (var token in tokens.OrderBy(x => x.Position))
            {
                if (token.Type == TokenType.Italic)
                {
                    if (token.Element == TokenElement.Close)
                        lastOpenIsItalics = false;
                    else
                    {
                        lastOpenIsItalics = true;
                    }
                }

                if (lastOpenIsItalics && token.Type == TokenType.Strong)
                {
                    token.SetToDefault();
                    yield return token;
                    continue;
                }


                yield return token;
            }
        }

        private static bool HaveDigit(string MdString, Token token)
        {
            if (token.Position - 1 >= 0 && digits.Contains(MdString[token.Position - 1]))
                return true;
            if (token.Position + token.Length < MdString.Length &&
                digits.Contains(MdString[token.Position + token.Length]))
                return true;
            return false;
        }
    }
}
