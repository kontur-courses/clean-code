using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class Md
    {
        private readonly Tokenizer tokenizer;

        
        public Md()
        {
            tokenizer = new Tokenizer(
                GetSettings(),
                GetTokenRules(),
                GetGroupTokenRules());
        }

        
        public string Render(string text)
        {
            return tokenizer.Tokenize(text).Render();
        }

        private static IEnumerable<MdWrapSetting> GetSettings()
        {
            yield return new MdWrapSetting("#", "<h1>", "</h1>", MdTagType.Block,
                new Func<string, Token, bool>[] {IsIgnoreTagWhenNotStartWithNewLine});
            yield return new MdWrapSetting("_", "<em>", "</em>", MdTagType.Span,
                new Func<string, Token, bool>[] {IsIgnoreTagWhenPlacedWithNumbers});
            yield return new MdWrapSetting("__", "<strong>", "</strong>", MdTagType.Span,
                new Func<string, Token, bool>[] {IsIgnoreTagWhenPlacedWithNumbers});
        }

        private static IEnumerable<Func<string, Token, bool>> GetTokenRules()
        {
            yield return IsIgnoreSpanTagsInDifferentWords;
            yield return IsIgnoreSpanTagsInDifferentLines;
            yield return IsIgnoreEmptyTokens;
        }

        private static IEnumerable<Func<Token, IEnumerable<Token>, bool>> GetGroupTokenRules()
        {
            yield return IsIgnoreSpanTagWhenParentTagLengthIsGreater;
            yield return IsIgnoreIntersectSpanTags;
        }

        private static bool IsIgnoreTagWhenNotStartWithNewLine(string text, Token tag)
        {
            return !(text.IsSubstring(tag.Position, Environment.NewLine, false) || tag.Position == 0);
        }

        private static bool IsIgnoreTagWhenPlacedWithNumbers(string text, Token tag)
        {
            return text.IsSubstring(tag.Position, char.IsDigit, false) == true ||
                   text.IsSubstring(tag.Position + tag.Value.Length, char.IsDigit) == true;
        }

        private static bool IsIgnoreSpanTagsInDifferentLines(string text, Token token)
        {
            return token.WrapSetting.TagType == MdTagType.Span && token.Text.Split(Environment.NewLine).Length != 1;
        }

        private static bool IsIgnoreSpanTagsInDifferentWords(string text, Token token)
        {
            if (token.WrapSetting.TagType != MdTagType.Span || token.Text.Split(' ').Length == 1)
                return false;

            return !(text.IsSubstring(token.Position, char.IsWhiteSpace, false) != false &&
                     text.IsSubstring(token.Position + token.Value.Length, char.IsWhiteSpace) != false);
        }

        private static bool IsIgnoreEmptyTokens(string text, Token token)
        {
            return string.IsNullOrEmpty(token.Text);
        }

        private static bool IsIgnoreSpanTagWhenParentTagLengthIsGreater(Token checkToken, IEnumerable<Token> tokens)
        {
            return checkToken.WrapSetting.TagType == MdTagType.Span &&
                   tokens.Where(token => token != checkToken)
                       .Where(token => token.WrapSetting.TagType == MdTagType.Span)
                       .Select(parent => parent.IsChild(checkToken) &&
                                         !(parent.WrapSetting.MdTag.Length > checkToken.WrapSetting.MdTag.Length))
                       .FirstOrDefault();
        }

        private static bool IsIgnoreIntersectSpanTags(Token checkToken, IEnumerable<Token> tokens)
        {
            static bool IsIntersect(Token token1, Token token2) =>
                token1.Position > token2.Position &&
                token1.Position < token2.Position + token2.Value.Length
                && token1.Position + token1.Value.Length > token2.Position + token2.Value.Length;

            return checkToken.WrapSetting.TagType == MdTagType.Span &&
                   tokens.Where(token => token != checkToken)
                       .Where(token => token.WrapSetting.TagType == MdTagType.Span)
                       .Select(token => IsIntersect(checkToken, token) || IsIntersect(token, checkToken))
                       .Any(result => result);
        }
    }
}