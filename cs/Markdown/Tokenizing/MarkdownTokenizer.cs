using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Languages;
using JetBrains.Annotations;

namespace Markdown.Tokenizing
{
    public class MarkdownTokenizer
    {
        private static readonly Language Markdown = new MarkdownLanguage();
        private static int MaxTagLength => Markdown.MaxTagLength;


        public static List<Token> Tokenize([NotNull] string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source can't be null or empty", nameof(source));

            var tokens = new List<Token>();
            var currentIndex = 0;
            var rawContent = new StringBuilder();

            while (currentIndex < source.Length)
            {
                if (!CharacterIsEscaped(currentIndex - 1, source) && TryReadTag(currentIndex, source, out var tag))
                {
                    if (rawContent.Length != 0)
                        tokens.Add(ParseRawToken(rawContent));

                    var token = ParseToken(currentIndex, source, tag);
                    tokens.Add(token);
                    currentIndex += tag.Length;
                    continue;
                }

                if (source[currentIndex] != '\\')
                    rawContent.Append(source[currentIndex]);
                currentIndex++;
            }

            if (rawContent.Length != 0)
                tokens.Add(ParseRawToken(rawContent));

            var unpairedTokens = FilterUnpairedTokens(tokens);
            ConvertTokensToRaw(unpairedTokens);
            return ConcatRawTokens(tokens);
        }

        private static bool TryReadTag(int start, string source, out string tag)
        {
            var possibleTag = Substring(start, source);

            tag = Markdown.OpeningTags
                .Where(pair => possibleTag.StartsWith(pair.Value))
                .OrderByDescending(pair => pair.Value.Length)
                .FirstOrDefault()
                .Value;

            return !string.IsNullOrEmpty(tag);
        }

        private static IEnumerable<Token> FilterUnpairedTokens(IEnumerable<Token> tokens)
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

        private static List<Token> ConcatRawTokens(List<Token> tokens)
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

        private static void ConvertTokensToRaw(IEnumerable<Token> tokensToConvert)
        {
            foreach (var token in tokensToConvert)
            {
                token.Content = Markdown.OpeningTags.First(pair => pair.Key == token.Tag).Value;
                token.Tag = Tag.Raw;
            }
        }

        private static Token ParseRawToken(StringBuilder stringBuilder)
        {
            var token = new Token(Tag.Raw, false, stringBuilder.ToString());
            stringBuilder.Clear();
            return token;
        }

        private static Token ParseToken(int index, string source, string tag)
        {
            var tagIsOpening = !CharacterIsWhiteSpace(index + tag.Length, source);
            var tagIsClosing = !CharacterIsWhiteSpace(index - 1, source);

            if (tagIsOpening && tagIsClosing || !tagIsOpening && !tagIsClosing)
                return new Token(Tag.Raw, true, tag);

            return new Token(Markdown.OpeningTags.First(pair => pair.Value == tag).Key, tagIsOpening);
        }

        private static bool CharacterIsEscaped(int index, string source)
        {
            return index > 0 && source[index] == '\\';
        }

        private static bool CharacterIsWhiteSpace(int index, string source)
        {
            return index < 0 || index >= source.Length || char.IsWhiteSpace(source[index]);
        }

        private static string Substring(int start, string source)
        {
            return source.Substring(start, Math.Min(MaxTagLength, source.Length - start));
        }
    }
}