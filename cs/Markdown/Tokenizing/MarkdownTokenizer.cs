using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Languages;

namespace Markdown.Tokenizing
{
    public class MarkdownTokenizer
    {
        private readonly Language language;
        private readonly int maxTagLength;
        private readonly Stack<Token> tokenStack;

        private int currentIndex;
        private readonly string source;

        public MarkdownTokenizer(string source)
        {
            language = new MarkdownLanguage();
            maxTagLength = language.MaxTagLength;
            tokenStack = new Stack<Token>();

            currentIndex = 0;
            this.source = source;
        }

        public static List<Token> Tokenize(string source)
        {
            return new MarkdownTokenizer(source).Tokenize();
        }

        private List<Token> Tokenize()
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Should not be null or empty", nameof(source));

            var result = new List<Token>();
            var rawTagStart = 0;

            for (var i = 0; i < source.Length; i++)
            {
                if (TryParseToken(source.Substring(i, Math.Min(source.Length - i, maxTagLength)), out var token))
                {
                    if (i != rawTagStart)
                        result.Add(new Token(Tag.Raw, false, source.Substring(rawTagStart, i - rawTagStart)));
                    result.Add(token);

                    if (token.IsOpening)
                        tokenStack.Push(token);
                    else tokenStack.Pop();

                    var tokenLength = GetTokenLength(token);
                    i += tokenLength - 1;
                    rawTagStart = i + 1;
                }
            }

            if (rawTagStart != source.Length)
                result.Add(new Token(Tag.Raw, false, source.Substring(rawTagStart, source.Length - rawTagStart)));

            return result;
        }

        public bool TryParseToken(string source, out Token token)
        {
            for (var length = source.Length; length > 0; length--)
            {
                if (language.TryParseOpeningTag(source.Substring(0, length), out var tag))
                {
                    var isOpening = true;

                    if (tokenStack.Count != 0)
                    {
                        var lastToken = tokenStack.Peek();
                        isOpening = !(language.ConvertOpeningTag(tag) == language.ConvertClosingTag(tag) && lastToken.Tag == tag && lastToken.IsOpening);
                    }

                    token = new Token(tag, isOpening);
                    return true;
                }

                if (language.TryParseClosingTag(source.Substring(0, length), out tag))
                {
                    token = new Token(tag, false);
                    return true;
                }
            }

            token = null;
            return false;
        }

        private int GetTokenLength(Token token)
        {
            return (token.IsOpening ? language.ConvertOpeningTag(token.Tag) : language.ConvertClosingTag(token.Tag))
                .Length;
        }

        #region New

        private IEnumerable<char> ReadUntilTag()
        {
            var substring = source.Substring(currentIndex);

            if (language.OpeningTags.Any(p => substring.StartsWith(p.Value)) ||
                language.ClosingTags.Any(p => substring.StartsWith(p.Value)))
                yield break;
            yield return source[currentIndex++];
        }

        #endregion

    }
}