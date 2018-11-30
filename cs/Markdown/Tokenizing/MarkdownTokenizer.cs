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
                if (!CharacterIsEscaped(currentIndex, source) && TryReadTag(currentIndex, source, out var tag))
                {
                    if (rawContent.Length != 0)
                        tokens.Add(CreateRawToken(rawContent));

                    var token = ParseToken(currentIndex, source, tag);
                    tokens.Add(token);
                    currentIndex += tag.Length;
                    continue;
                }

                if (source[currentIndex] != Markdown.EscapeCharacter)
                    rawContent.Append(source[currentIndex]);
                currentIndex++;
            }

            if (rawContent.Length != 0)
                tokens.Add(CreateRawToken(rawContent));

            tokens = ConvertToRawInsideEmphasizeTags(tokens);
            var unpairedTokens = new HashSet<Token>(FilterUnpairedTokens(tokens));

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (unpairedTokens.Contains(token))
                {
                    var rawToken = ConvertTokenToRaw(token);
                    tokens[i] = rawToken;
                }
            }

            return ConcatRawTokens(tokens);
        }

        private static bool TryReadTag(int start, string source, out string tag)
        {
            var possibleTag = GetPossibleTag(start, source);

            tag = Markdown.OpeningTags
                .Values
                .Where(t => possibleTag.StartsWith(t))
                .OrderByDescending(t => t.Length)
                .FirstOrDefault();

            return !string.IsNullOrEmpty(tag);
        }

        private static List<Token> ConvertToRawInsideEmphasizeTags(IEnumerable<Token> tokens)
        {
            var tokensList = tokens.ToList();
            var inEmphasizeTag = false;
            var indexesToConvert = new List<int>();

            for (var i = 0; i < tokensList.Count; i++)
            {
                if (tokensList[i].Tag == Tag.Raw)
                    continue;

                if (tokensList[i].Tag == Tag.Emphasize && tokensList[i].IsOpening)
                {
                    inEmphasizeTag = true;
                    continue;
                }

                if (tokensList[i].Tag == Tag.Emphasize && !tokensList[i].IsOpening)
                {
                    inEmphasizeTag = false;

                    foreach (var j in indexesToConvert)
                    {
                        tokensList[j] = ConvertTokenToRaw(tokensList[j]);
                    }

                    indexesToConvert.Clear();

                    continue;
                }

                if (inEmphasizeTag)
                    indexesToConvert.Add(i);
            }

            return tokensList;
        }

        private static IEnumerable<Token> FilterUnpairedTokens(IEnumerable<Token> tokens)
        {
            var tokenStacks = new Dictionary<Tag, Stack<Token>>
            {
                {Tag.Strong, new Stack<Token>()},
                {Tag.Emphasize, new Stack<Token>()},
                {Tag.Pre, new Stack<Token>()}
            };

            Tag previousTag = Tag.Raw;

            foreach (var token in tokens.Where(t => t.Tag != Tag.Raw))
            {
                var stack = tokenStacks[token.Tag];

                if (token.IsOpening)
                {
                    stack.Push(token);
                    previousTag = token.Tag;
                    continue;
                }

                if (previousTag == token.Tag && stack.Peek().IsOpening)
                {
                    stack.Pop();
                    previousTag = Tag.Raw;
                    continue;
                }

                if (stack.Any() && stack.Peek().IsOpening)
                {
                    stack.Pop();
                    continue;
                }

                stack.Push(token);
                previousTag = token.Tag;
            }

            return tokenStacks.Values.SelectMany(stack => stack);
        }

        private static List<Token> ConcatRawTokens(List<Token> tokens)
        {
            var result = new List<Token>();
            var rawContent = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token.Tag == Tag.Raw)
                {
                    rawContent.Append(token.Content);
                    continue;
                }

                if (rawContent.Length != 0)
                    result.Add(CreateRawToken(rawContent));
                result.Add(token);
            }

            if (rawContent.Length != 0)
                result.Add(CreateRawToken(rawContent));

            return result;
        }

        private static Token ConvertTokenToRaw(Token token)
        {
            var tag = Markdown.OpeningTags.First(pair => pair.Key == token.Tag).Value;
            return new Token(Tag.Raw, false, tag);
        }

        private static Token CreateRawToken(StringBuilder stringBuilder)
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
            return index > 0 && source[index - 1] == Markdown.EscapeCharacter;
        }

        private static bool CharacterIsWhiteSpace(int index, string source)
        {
            return index < 0 || index >= source.Length || char.IsWhiteSpace(source[index]);
        }

        private static string GetPossibleTag(int start, string source)
        {
            return source.Substring(start, Math.Min(MaxTagLength, source.Length - start));
        }
    }
}