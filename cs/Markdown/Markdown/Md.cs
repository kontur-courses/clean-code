using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    public class Md
    {
        private readonly Tokenizer tokenizer;

        public Md()
        {
            tokenizer = new Tokenizer(GetSettings(), GetMdSpecificationRules());
        }

        public string Render(string text)
        {
            var token = tokenizer.Tokenize(text);
            return token.Render();
        }

        private static IEnumerable<MdWrapSetting> GetSettings()
        {
            yield return new MdWrapSetting("#", "<h1>", "</h1>", MdTagType.Block);
            yield return new MdWrapSetting("_", "<em>", "</em>", MdTagType.Span);
            yield return new MdWrapSetting("__", "<strong>", "</strong>", MdTagType.Span);
        }

        private static IEnumerable<Action<Token>> GetMdSpecificationRules()
        {
            yield return IgnoreBlockTagWhenNotStartWithNewLine;
            yield return IgnoreSpanTagWhenParentTagLengthIsGreater;
            yield return IgnoreSpanTagWhenPlacedWithNumbers;
            yield return IgnoreSpanTagsInDifferentWords;
            //yield return IgnoreSpanTagsWhenIntersect;
            yield return IgnoreEmptyTokens;
        }

        private static void IgnoreBlockTagWhenNotStartWithNewLine(Token token)
        {
            if (token.WrapSetting.TagType != MdTagType.Block)
                return;

            var parent = token.Parent ?? token;
            token.IsIgnore = !(parent.Text.IsSubstring(token.Position, Environment.NewLine, false) ||
                               token.GetRoot == token.Parent && token.Position == 0);
        }

        private static void IgnoreSpanTagWhenParentTagLengthIsGreater(Token token)
        {
            var parent = token.Parent ?? token;
            if (token.WrapSetting.TagType != MdTagType.Span || parent.WrapSetting.TagType != MdTagType.Span)
                return;
            token.IsIgnore = !(parent.WrapSetting.MdTag.Length > token.WrapSetting.MdTag.Length);
        }

        private static void IgnoreSpanTagWhenPlacedWithNumbers(Token token)
        {
            if (token.WrapSetting.TagType != MdTagType.Span)
                return;

            var parent = token.Parent ?? token;
            token.IsIgnore = parent.Text.IsSubstring(token.Position, char.IsDigit, false) == true ||
                             parent.Text.IsSubstring(
                                 token.Position + token.WrapSetting.MdTag.Length, char.IsDigit) == true ||
                             parent.Text.IsSubstring(token.Position + token.Value.Length, char.IsDigit) == true ||
                             parent.Text.IsSubstring(
                                 token.Position + token.Value.Length - token.WrapSetting.MdCloseTag.Length,
                                 char.IsDigit, false) == true;
        }

        private static void IgnoreSpanTagsInDifferentWords(Token token)
        {
            if (token.WrapSetting.TagType != MdTagType.Span || token.Text.Split(' ').Length == 1)
                return;

            var parent = token.Parent ?? token;
            token.IsIgnore = !(parent.Text.IsSubstring(token.Position, char.IsWhiteSpace, false) != false &&
                               parent.Text.IsSubstring(token.Position + token.Value.Length, char.IsWhiteSpace) !=
                               false);
        }

        private static void IgnoreSpanTagsWhenIntersect(Token token)
        {
            static bool IsIntersect(Token token1, Token token2) =>
                token1.Position > token2.Position &&
                token1.Position < token2.Position + token2.Value.Length
                && token1.Position + token1.Value.Length > token2.Position + token2.Value.Length;

            var parent = token.Parent ?? token;
            foreach (var child in parent.AllDescendants
                .Where(child => child.WrapSetting.TagType == MdTagType.Span)
                .Where(child => IsIntersect(token, child)))
            {
                child.IsIgnore = true;
                token.IsIgnore = true;
            }
        }

        private static void IgnoreEmptyTokens(Token token)
        {
            token.IsIgnore = string.IsNullOrEmpty(token.Text);
        }
    }
}