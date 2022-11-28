namespace Markdown.Tokens
{
    public static class IdentifierTokenElement
    {
        public static IEnumerable<Token> GetPairs(Stack<Token> unknownStackTags, Token token)
        {
            if (unknownStackTags.Count > 0 && unknownStackTags.Peek().Type == token.Type)
            {
                foreach (var pairToken in GetPair(unknownStackTags, token))
                    yield return pairToken;
                yield break;
            }
            token.SetToDefault();
            yield return token;
        }

        private static IEnumerable<Token> GetPair(Stack<Token> unknownStackTags, Token token)
        {
            unknownStackTags.Peek().Element = TokenElement.Open;
            yield return unknownStackTags.Pop();
            yield return token;
        }

        public static IEnumerable<Token> AddUndefinedToken(Stack<Token> undefinedStackTags, string md, Stack<Token> tagsStack, Token token)
        {
            if (tagsStack.Count > 0 && tagsStack.Peek().Type == token.Type)
            {
                if (!md.Substring(tagsStack.Peek().End + 1,
                        token.Position - tagsStack.Peek().End - 1).Contains(' '))
                {
                    token.Element = TokenElement.Close;
                    yield return tagsStack.Pop();
                    yield return token;
                }
                else
                    undefinedStackTags.Push(token);
            }
            else
                undefinedStackTags.Push(token);
        }
        public static IEnumerable<Token> TokensWithPair(string md, Token token, Stack<Token> stackTokens,
            Stack<Token> unknownTags)
        {
            switch (token.Element)
            {
                case TokenElement.Open:
                    stackTokens.Push(token);
                    break;
                case TokenElement.Close:
                    foreach (var closeTag in stackTokens.CloseTags(unknownTags, token))
                        yield return closeTag;
                    break;
                case TokenElement.Unknown:
                    foreach (var unknownTag in unknownTags.UnknownTags(md, stackTokens, token))
                        yield return unknownTag;
                    break;
                case TokenElement.Default:
                default:
                    yield return token;
                    break;
            }
        }
    }
}
