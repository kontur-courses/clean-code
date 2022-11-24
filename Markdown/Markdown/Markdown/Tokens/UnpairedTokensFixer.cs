using Markdown.Markdown;


namespace Markdown.Tokens
{
    public static class UnpairedTokensFixer
    {
        public static IEnumerable<Token> RemoveSingleTokens(this IEnumerable<Token> tokens, string markdownString)
        {
            var unknownTags = new Stack<Token>();
            var stackTokens = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Header)
                {
                    yield return token;
                    continue;
                }

                if (token.Type == TokenType.Default || token.Type == TokenType.Unseen)
                {
                    foreach (var textToken in GetTextToken(markdownString, token, unknownTags))
                        yield return textToken;
                    yield return token;
                    continue;
                }

                foreach (var tokenMd in EnumerableTokens(markdownString, token, stackTokens, unknownTags))
                    yield return tokenMd;
            }

            foreach (var singleToken in ToStringTokensWithoutPair(stackTokens, unknownTags))
                yield return singleToken;

        }

        private static IEnumerable<Token> ToStringTokensWithoutPair(Stack<Token> stackTokens, Stack<Token> unknownTags)
        {
            foreach (var token in stackTokens.Concat(unknownTags))
            {
                token.SetToDefault();
                yield return token;
            }
        }

        private static IEnumerable<Token> EnumerableTokens(string md, Token token, Stack<Token> stackTokens,
            Stack<Token> unknownTags)
        {
            if (token.Element == TokenElement.Open)
                stackTokens.Push(token);
            else if (token.Element == TokenElement.Close)
                foreach (var closeTag in stackTokens.CloseTags(unknownTags, token))
                    yield return closeTag;
            else if (token.Element == TokenElement.Unknown)
                foreach (var unknownTag in unknownTags.UnknownTags(md, stackTokens, token))
                    yield return unknownTag;
            else
                yield return token;
        }

        private static IEnumerable<Token> GetTextToken(string md, Token token, Stack<Token> unknownTags)
        {
            var mdStringBetweenTags = md.Substring(token.Position, token.Length);
            if (!mdStringBetweenTags.Contains(' '))
                yield break;
            foreach (var unknownTag in unknownTags)
            {
                unknownTag.SetToDefault();
                yield return unknownTag;
            }
        }
    }
}
