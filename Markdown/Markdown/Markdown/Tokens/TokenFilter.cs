using Markdown.Markdown;

namespace Markdown.Tokens
{
    public static class TokenFilter
    {
        private static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static IList<Token> Filter(this IList<Token> tokens, string MarkdownString)
        {
            if (tokens.Count == 0)
                throw new ArgumentException("Empty tokens must not been filtered");
            return tokens
                .RemoveFields()
                .RemoveDigits(MarkdownString)
                .RemoveSingleTokens(MarkdownString)
                .RemoveStrongInItalics()
                .DistinctBy(x => x.Position).ToList()
                .InitializeImage(MarkdownString);
        }

        private static IEnumerable<Token> RemoveFields(this IList<Token> tokens)
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
        public static bool IsImage(this Token imageToken)
        {
            if (imageToken.Type == TokenType.Image || imageToken.Type == TokenType.ImageDescription ||
                imageToken.Type == TokenType.ImageStart)
                return true;
            return false;
        }

        private static IEnumerable<Token> RemoveDigits(this IEnumerable<Token> tokens, string mdString)
        {
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default|| token.IsImage())
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
                        lastOpenIsItalics = true;
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
        public static IList<Token> InitializeImage(this IList<Token> tokens, string mdString)
        {
            var images = tokens.Select(x => x).Where(x => x.IsImage()).ToList();
            for (var i = 0; i < images.Count; i++)
            {
                if (images[i].Type == TokenType.ImageStart)
                {
                    if (IsImage(i, images))
                        AddImageToken(tokens, mdString, images, i);
                    else
                        images[i].SetToDefault();
                }
                else if (images[i].Type != TokenType.Unseen)
                    images[i].SetToDefault();
            }
            return tokens.OrderBy(x => x.Position).ToList();
        }

        private static bool IsImage(int i, List<Token> images)
        {
            return i + 4 < images.Count && images[i + 1].Type == TokenType.ImageDescription &&
                   images[i + 2].Type == TokenType.ImageDescription && images[i + 3].Type == TokenType.Image &&
                   images[i + 4].Type == TokenType.Image && images[i].End + 1 == images[i + 1].Position && images[i + 2].End + 1 == images[i + 3].Position;
        }

        private static void AddImageToken(IList<Token> tokens, string mdString, List<Token> images, int i)
        {
            foreach (var token in tokens.Select(x => x)
                         .Where(x => x.Position >= images[i].Position
                                     && x.Position <= images[i + 4].End))
                token.SetToUnsee();
            var position = images[i].Position;
            var description = images[i + 1].GetTokenBetween(images[i + 2]).CreateString(mdString)
                .ToString();
            var path = images[i + 3].GetTokenBetween(images[i + 4]).CreateString(mdString).ToString();
            tokens.Add(new Token(position, description, path));
        }
    }
}
