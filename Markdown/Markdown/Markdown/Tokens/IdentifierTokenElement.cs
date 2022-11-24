using Markdown.Markdown;

namespace Markdown.Tokens
{
    public static class IdentifierTokenElement
    {
        public static IEnumerable<Token> CloseTags(this Stack<Token> stackTags, Stack<Token> unknownStackTags,
            Token token)
        {
            if (stackTags.Count <= 0)
            {
                if (unknownStackTags.Count > 0 && unknownStackTags.Peek().Type == token.Type)
                {
                    unknownStackTags.Peek().Element = TokenElement.Open;
                    yield return unknownStackTags.Pop();
                    yield return token;
                    yield break;
                }

                token.SetToDefault();
                yield return token;
                yield break;
            }

            var last = stackTags.Pop();
            if (last.Type != token.Type)
            {
                last.SetToDefault();
                token.SetToDefault();
            }

            yield return last;
            yield return token;
        }

        public static IEnumerable<Token> UnknownTags(this Stack<Token> undefinedStackTags, string md,
            Stack<Token> tagsStack, Token token)
        {
            if (undefinedStackTags.Count == 0)
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
            else if (undefinedStackTags.Peek().Type == token.Type)
            {
                undefinedStackTags.Peek().Element = TokenElement.Open;
                token.Element = TokenElement.Close;
                yield return undefinedStackTags.Pop();
                yield return token;
            }
            else
                undefinedStackTags.Push(token);
        }
    }
}
