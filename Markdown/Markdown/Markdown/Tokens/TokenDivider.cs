using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tokens
{
    public static class TokenDivider
    {
        public static IEnumerable<Token> CloseTags(this Stack<Token> stackTags, Stack<Token> unknownStackTags,
            Token token)
        {
            if (stackTags.Count <= 0)
            {
                foreach (var pairTokens in IdentifierTokenElement.GetPairs(unknownStackTags, token))
                    yield return pairTokens;
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
                foreach (var unknownTags in IdentifierTokenElement.AddUndefinedToken(undefinedStackTags, md, tagsStack, token))
                    yield return unknownTags;
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
