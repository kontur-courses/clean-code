using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Languages;
using JetBrains.Annotations;

namespace Markdown.Tokenizing
{
    public class MarkdownTokenizer
    {
        private readonly Language markdown;
        private readonly int maxTagLength;

        private int currentIndex;
        private readonly string source;

        public MarkdownTokenizer(string source)
        {
            markdown = new MarkdownLanguage();
            maxTagLength = markdown.MaxTagLength;

            currentIndex = 0;
            this.source = source;
        }

        public static List<Token> Tokenize([NotNull] string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source can't be null or empty", nameof(source));

            return new MarkdownTokenizer(source).Tokenize();
        }

        private List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (currentIndex < source.Length)
            {
                var rawContent = string.Concat(ReadUntilTag());
                if (!string.IsNullOrEmpty(rawContent))
                    tokens.Add(new Token(Tag.Raw, false, rawContent));

                var tag = string.Concat(ReadTag());
                if (string.IsNullOrEmpty(tag))
                    continue;

                Token newToken;
                if (TagIsOpening(tag))
                    newToken = ParseToken(tag, true);
                else if (TagIsClosing(tag))
                    newToken = ParseToken(tag, false);
                else newToken = new Token(Tag.Raw, false, tag);

                tokens.Add(newToken);
            }

            var unpairedTokens = FilterUnpairedTokens(tokens);
            ConvertTokensToRaw(unpairedTokens);
            return ConcatRawTokens(tokens);
        }

        private IEnumerable<Token> FilterUnpairedTokens(IEnumerable<Token> tokens)
        {
            var stack = new Stack<Token>();
            foreach (var token in tokens.Where(t => t.Tag != Tag.Raw))
            {
                if (token.IsOpening)
                    stack.Push(token);
                else if (stack.Count == 0)
                    stack.Push(token);
                else
                {
                    var previousToken = stack.Peek();
                    if (previousToken.Tag == token.Tag && previousToken.IsOpening)
                        stack.Pop();
                    else
                        stack.Push(token);
                }
            }

            return stack;
        }

        private List<Token> ConcatRawTokens(List<Token> tokens)
        {
            var result = new List<Token>();

            foreach (var token in tokens)
            {
                if (result.Count == 0)
                {
                    result.Add(token);
                    continue;
                }

                var lastAddedToken = result.Last();
                if (token.Tag == Tag.Raw && lastAddedToken.Tag == Tag.Raw)
                    lastAddedToken.Content += token.Content;
                else
                    result.Add(token);
            }

            return result;
        }

        private void ConvertTokensToRaw(IEnumerable<Token> tokensToConvert)
        {
            foreach (var token in tokensToConvert)
            {
                token.Content = markdown.OpeningTags.First(pair => pair.Key == token.Tag).Value;
                token.Tag = Tag.Raw;
            }
        }

        private Token ParseToken(IEnumerable<char> tag, bool opening)
        {
            var stringTag = string.Concat(tag);
            return new Token(markdown.OpeningTags.First(pair => pair.Value == stringTag).Key, opening);
        }

        private IEnumerable<char> ReadUntilTag()
        {
            while (!SubstringStartsWithTag)
            {
                if (currentIndex == source.Length)
                    yield break;
                if (source[currentIndex] != '\\')
                    yield return source[currentIndex];
                currentIndex++;
            }
        }

        private IEnumerable<char> ReadTag()
        {
            var currentSubstring = Substring;

            if (CurrentCharacterIsEscaped)
                yield break;

            var maxLengthTag = markdown.OpeningTags
                .Where(pair => currentSubstring.StartsWith(pair.Value))
                .OrderByDescending(pair => pair.Value.Length)
                .FirstOrDefault()
                .Value;

            if (string.IsNullOrEmpty(maxLengthTag))
                yield break;

            foreach (var letter in maxLengthTag)
            {
                currentIndex++;
                yield return letter;
            }
        }

        private bool CurrentCharacterIsEscaped =>
            currentIndex > 0 && source[currentIndex - 1] == '\\';

        private string Substring =>
            source.Substring(currentIndex, Math.Min(maxTagLength, source.Length - currentIndex));

        private bool SubstringStartsWithTag =>
            markdown.OpeningTags.Any(pair => Substring.StartsWith(pair.Value)) && !CurrentCharacterIsEscaped;

        private bool TagIsOpening(string tag)
        {
            return !CharacterIsWhiteSpace(currentIndex);
        }

        private bool TagIsClosing(string tag)
        {
            return !CharacterIsWhiteSpace(currentIndex - 1 - tag.Length);
        }

        private bool CharacterIsWhiteSpace(int index)
        {
            return index < 0 || index >= source.Length || char.IsWhiteSpace(source[index]);
        }
    }
}