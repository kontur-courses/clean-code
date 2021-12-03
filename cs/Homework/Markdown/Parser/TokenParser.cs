using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.TokenIdentifiers;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class TokenParser : IParser<MarkdownToken>
    {
        private readonly Dictionary<string, TokenIdentifier<MarkdownToken>> identifiers;

        private readonly Dictionary<string, SelectorType> tagTypes = new()
        {
            ["_"] = SelectorType.Double,
            ["__"] = SelectorType.Double,
            ["#"] = SelectorType.Line,
            
        };


        private readonly char screeningSymbol = '\\';
        private readonly int maxTagLength;


        public TokenParser()
        {
            identifiers = new Dictionary<string, TokenIdentifier<MarkdownToken>>()
            {
                ["_"] = new ItalicTokenIdentifier(this, "_"),
                ["__"] = new StrongTokenIdentifier(this, "__"),
                ["#"] = new HeaderTokenIdentifier(this, "#")
            };
            maxTagLength = GetMaxTagLength();
        }

        public List<MarkdownToken> Parse(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            var tokensFromText = new List<MarkdownToken>();
            var paragraphs = SplitSavingSeparator(text, '\n');
            for ( var paragraphIndex = 0; paragraphIndex < paragraphs.Length; paragraphIndex++)
            {
                foreach (var token in GetTokensFromParagraph(paragraphs, paragraphIndex))
                    tokensFromText.Add(token);
            }
            return tokensFromText;
        }

        private static string[] SplitSavingSeparator(string text, char separator)
        {
            var splitedText = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (splitedText.Length == 1)
                return new []{text};

            return splitedText
                .SkipLast(1)
                .Select(str => str + separator)
                .Concat(splitedText.TakeLast(1))
                .ToArray();
        }


        private IEnumerable<MarkdownToken> GetTokensFromParagraph(string[] paragraphs, int paragraphIndex)
        {
            var currentParagraph = paragraphs[paragraphIndex];
            var position = 0;
            var plainText = new StringBuilder();
            var isScreening = false;
            while (position < currentParagraph.Length)
            {
                isScreening = !isScreening ? IsScreeningAt(currentParagraph, position) : isScreening;
                var tag = TryGetTagInPosition(currentParagraph, position);
                MarkdownToken token = null;
                if (tag != null)
                {
                    if (!isScreening)
                        token = FindTokenFromPosition(paragraphs, paragraphIndex, position, tag);
                    else
                        isScreening = false;
                }

                if (tag == null || token == null)
                {
                    if(!isScreening)
                        plainText.Append(tag == null 
                            ? currentParagraph[position]
                            : currentParagraph[position..(position + tag.Length)]);
                    if (position + 1 < currentParagraph.Length && currentParagraph[position + 1] == '\\')
                        isScreening = false;
                    position += tag?.Length ?? 1;
                    continue;
                }

                if (plainText.Length != 0)
                    yield return new PlainTextToken(plainText.ToString(), null, paragraphIndex,
                        position - plainText.Length);
                plainText.Clear();
                yield return token;
                position += token.Value.Length;
            }
            if (plainText.Length != 0)
                yield return new PlainTextToken(plainText.ToString(), null, paragraphIndex,
                    position - plainText.Length);
        }

        private bool IsScreeningAt(string currentParagraph, int position)
        {
            var nextPossibleSelector = position != currentParagraph.Length - 1
            ? TryGetTagInPosition(currentParagraph, position + 1)
            : null;
            char? next = position != currentParagraph.Length - 1
                ? currentParagraph[position + 1]
                : null;
            if (position < 0 || currentParagraph[position] != screeningSymbol)
                return false;
            return !IsScreeningAt(currentParagraph, position - 1)
                && nextPossibleSelector != null || next == screeningSymbol;
        }


        private MarkdownToken FindTokenFromPosition(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            return tagTypes[tag] switch
            {
                SelectorType.Double => TryReadDoubleTagToken(paragraphs, paragraphIndex, position, tag),
                SelectorType.Line => TryReadLineToken(paragraphs, paragraphIndex, position, tag),
                _ => null
            };
        }


        private MarkdownToken TryReadDoubleTagToken(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            var paragraph = paragraphs[paragraphIndex];
            var closeTagIndex = IndexOfCloseTag(paragraph, tag, position);
            if (closeTagIndex == -1) return null;
            var tokenValue = paragraph[position..(closeTagIndex + tag.Length)];
            var temporaryToken = new TemporaryToken(tokenValue, tag, paragraphIndex, position);
            identifiers[tag].Identify(paragraphs, temporaryToken, out var identifiedToken);
            return identifiedToken;
        }

        private MarkdownToken TryReadLineToken(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            var temporaryToken = new TemporaryToken(paragraphs[paragraphIndex], tag, paragraphIndex, position);
            identifiers[tag].Identify(paragraphs, temporaryToken, out var identifiedToken);
                return identifiedToken;
        }

        private static int IndexOfCloseTag(string paragraph, string openTag, int openTagIndex)
        {
            var startIndex = openTagIndex + openTag.Length;
            return paragraph.IndexOf(openTag, startIndex);
        }

        private string TryGetTagInPosition(string paragraph, int position)
        {
            for (var i = maxTagLength; i > 0 && position + i < paragraph.Length; i--)
            {
                var slice = paragraph[position..(position + i)];
                if (identifiers.ContainsKey(slice))
                    return slice;
            }

            return null;
        }

        private int GetMaxTagLength()
        {
            return identifiers.Keys.Max(tag => tag.Length);
        }
    }
}