using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Languages;

namespace Markdown.Tokenizing
{
    public class MarkdownTokenizer
    {
        private readonly Language markdown;
        private readonly int maxTagLength;
        private readonly Stack<Token> tokenStack;
        private readonly List<Token> tokens;

        private int currentIndex;
        private readonly string source;

        public MarkdownTokenizer(string source)
        {
            markdown = new MarkdownLanguage();
            maxTagLength = markdown.MaxTagLength;
            tokenStack = new Stack<Token>();
            tokens = new List<Token>();

            currentIndex = 0;
            this.source = source;
        }

        public static List<Token> Tokenize(string source)
        {
            return new MarkdownTokenizer(source).Tokenize();
        }

        private List<Token> Tokenize()
        {
            while (currentIndex < source.Length)
            {
                var rawContent = string.Concat(ReadUntilTag());
                if (!string.IsNullOrEmpty(rawContent))
                    tokens.Add(new Token(Tag.Raw, false, rawContent));

                Token newToken;
                var tag = ReadTag();
                if (!NextCharacterIsWhiteSpace)
                    newToken = ParseToken(tag, true);
                else if (!PreviousCharacterIsWhiteSpace)
                    newToken = ParseToken(tag, false);
                else newToken = new Token(Tag.Raw, false, string.Concat(tag));

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
            var currentSubstring = Substring;

            if (markdown.OpeningTags.Any(p => currentSubstring.StartsWith(p.Value)) && !CurrentCharacterIsEscaped)
                yield break;
            yield return source[currentIndex++];
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

        private bool CurrentCharacterIsEscaped => currentIndex > 0 && source[currentIndex - 1] == '\\';

        private bool NextCharacterIsWhiteSpace =>
            currentIndex < source.Length - 1 && char.IsWhiteSpace(source[currentIndex + 1]);

        private bool PreviousCharacterIsWhiteSpace =>
            currentIndex > 0 && char.IsWhiteSpace(source[currentIndex - 1]);

        private string Substring => source.Substring(currentIndex, maxTagLength);
    }
}