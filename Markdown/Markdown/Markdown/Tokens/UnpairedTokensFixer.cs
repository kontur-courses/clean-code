namespace Markdown.Tokens
{
    public static class UnpairedTokensFixer
    {
        public static IEnumerable<Token> RemoveSingleTokens(this IEnumerable<Token> tokens, string markdownString)
        {
            var unknownTags = new Stack<Token>();
            var stackTokens = new Stack<Token>();
            foreach (var token in tokens)
            foreach (var singleTokens in RemoveUnpaired(markdownString, token, unknownTags, stackTokens))
                yield return singleTokens;
            foreach (var token in stackTokens.Concat(unknownTags))
            {
                token.SetToDefault();
                yield return token;
            }
        }

        private static IEnumerable<Token> RemoveUnpaired(string markdownString, Token token, Stack<Token> unknownTags,
            Stack<Token> stackTokens)
        {
            if (token.Type != TokenType.Header && !token.IsImage() && token.Type != TokenType.Unseen)
            {
                if (token.Type == TokenType.Default)
                {
                    foreach (var textToken in ReturnTextToken(markdownString, token, unknownTags))
                        yield return textToken;
                    yield break;
                }

                foreach (var tokenMd in IdentifierTokenElement.TokensWithPair(markdownString, token, stackTokens, unknownTags))
                    yield return tokenMd;
            }
            else
                yield return token;
        }

        private static IEnumerable<Token> ReturnTextToken(string markdownString, Token token, Stack<Token> unknownTags)
        {
            foreach (var textToken in GetTextTokenInDifferentWords(markdownString, token, unknownTags))
                yield return textToken;
            yield return token;
        }

        private static IEnumerable<Token> GetTextTokenInDifferentWords(string md, Token token, Stack<Token> unknownTags)
        {
            if (!md.AsSpan(token.Position, token.Length).Contains(' '))
                yield break;
            foreach (var unknownTag in unknownTags)
            {
                unknownTag.SetToDefault();
                yield return unknownTag;
            }
        }
    }
}
