using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Common;

namespace Markdown
{
    public class Md
    {
        private readonly Tokenizer tokenizer;

        public Md()
        {
            tokenizer = new Tokenizer(
                GetMdTags(),
                GetGroupTokenRules());
        }

        public string Render(string text)
        {
            var tokens = tokenizer.Tokenize(text);
            return tokens.Render();
        }

        private static IEnumerable<BaseMdTag> GetMdTags()
        {
            yield return new BlockMdTag("#", "<h1>", "</h1>");
            yield return new SpanMdTag("_", "<em>", "</em>");
            yield return new SpanMdTag("__", "<strong>", "</strong>");
            yield return new ListMdTag("+", "<ul>", "</ul>", "<li>", "</li>");
        }

        private static IEnumerable<Func<Token, IEnumerable<Token>, bool>> GetGroupTokenRules()
        {
            yield return IsIgnoreSpanTagWhenParentTagLengthIsGreater;
            yield return IsIgnoreIntersectSpanTags;
        }

        private static bool IsIgnoreSpanTagWhenParentTagLengthIsGreater(Token checkToken, IEnumerable<Token> tokens)
        {
            return checkToken.MdTag is SpanMdTag &&
                   tokens.Where(token => token != checkToken)
                       .Where(token => token.MdTag is SpanMdTag)
                       .Select(parent => parent.IsChild(checkToken) &&
                                         !(parent.MdTag.MdTag.Length > checkToken.MdTag.MdTag.Length))
                       .FirstOrDefault();
        }

        private static bool IsIgnoreIntersectSpanTags(Token checkToken, IEnumerable<Token> tokens)
        {
            static bool IsIntersect(Token token1, Token token2) =>
                token1.Position > token2.Position &&
                token1.Position < token2.Position + token2.Value.Length
                && token1.Position + token1.Value.Length > token2.Position + token2.Value.Length;

            return checkToken.MdTag is SpanMdTag &&
                   tokens.Where(token => token != checkToken)
                       .Where(token => token.MdTag is SpanMdTag)
                       .Select(token => IsIntersect(checkToken, token) || IsIntersect(token, checkToken))
                       .Any(result => result);
        }
    }
}