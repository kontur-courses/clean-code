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
        private readonly List<Token> tokens;

        private int currentIndex;
        private readonly string source;

        public MarkdownTokenizer(string source)
        {
            markdown = new MarkdownLanguage();
            maxTagLength = markdown.MaxTagLength;
            tokens = new List<Token>();

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

            return tokens;
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
                yield return source[currentIndex++];
            }
        }

        private IEnumerable<char> ReadTag()
        {
            var currentSubstring = Substring;

            if (CurrentCharacterIsEscaped)
                yield break;

            var maxLengthTag = markdown.OpeningTags
                .Where(pair => currentSubstring.StartsWith(pair.Value))
                .OrderBy(pair => pair.Value.Length)
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