using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.TokenIdentifiers;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class TokenParser : IParser
    {
        private readonly Dictionary<string, TokenIdentifier> identifiers = new()
        {
            ["_"] = new ItalicTokenIdentifier("_", t => new ItalicToken(t.Value, t.ParagraphIndex, t.StartIndex)),
            ["__"] = new BoldTokenIdentifier("__", t => new BoldToken(t.Value, t.ParagraphIndex, t.StartIndex)),
            ["#"] = new HeaderTokenIdentifier("#", t => new HeaderToken(t.Value, t.ParagraphIndex, t.StartIndex)),
        };

        private readonly Dictionary<string, TagType> tagTypes = new()
        {
            ["_"] = TagType.Double,
            ["__"] = TagType.Double,
            ["#"] = TagType.Line,
            
        };

        private readonly int maxTagLength;
        private readonly HashSet<string> openedTags = new();
        private readonly StringBuilder plainText = new();

        private bool isInsideTag => openedTags.Count > 0;
        

        public TokenParser()
        {
            maxTagLength = GetMaxTagLength();
        }

        public List<Token> Parse(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            var tokensFromText = new List<Token>();
            var paragraphs = SplitSavingSeparator(text, '\n');
            for ( var paragraphIndex = 0; paragraphIndex < paragraphs.Length; paragraphIndex++)
            {
                foreach (var token in GetTokensFromParagraph(paragraphs, paragraphIndex))
                    tokensFromText.Add(token);
                ResetTemporaryResources();
            }
            return tokensFromText;
        }

        private string[] SplitSavingSeparator(string text, char separator)
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


        private IEnumerable<Token> GetTokensFromParagraph(string[] paragraphs, int paragraphIndex)
        {
            var currentParagraph = paragraphs[paragraphIndex];
            var position = 0;
            while (position < currentParagraph.Length)
            {
                var tag = TryGetTagInPosition(currentParagraph, position);
                if (tag != null)
                {
                    if (openedTags.Contains(tag))
                    {
                        position += tag.Length;
                        if(tagTypes[tag] == TagType.Double)
                            openedTags.Remove(tag);
                        continue;
                    }
                    else
                    {
                        var token = FindTokenFromPosition(paragraphs, paragraphIndex, position, tag);
                        if (token != null)
                        {
                            if (plainText.Length > 0)
                                yield return new PlainTextToken(plainText.ToString(), paragraphIndex,
                                    position - plainText.Length);
                            plainText.Clear();
                            yield return token;
                            position += tag.Length;
                            continue;
                        }
                        plainText.Append(tag);
                        position += tag.Length;
                        continue;
                    }
                }
                else if (!isInsideTag)
                {
                    plainText.Append(currentParagraph[position]);
                }
                position++;
            }

            if (plainText.Length > 0)
            {
                yield return new PlainTextToken(plainText.ToString(), paragraphIndex,
                    position - plainText.Length);
                plainText.Clear();
            }
        }

        private Token FindTokenFromPosition(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            switch (tagTypes[tag])
            {
                case TagType.Double:
                    return TryReadDoubleTagToken(paragraphs, paragraphIndex, position, tag);
                case TagType.Line:
                    return TryReadLineToken(paragraphs, paragraphIndex, position, tag);
                default:
                    return null;
            }
        }

        private Token TryReadDoubleTagToken(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            if (openedTags.Contains(tag)) return null;
            var paragraph = paragraphs[paragraphIndex];
            var closeTagIndex = IndexOfCloseTag(paragraph, tag, position);
            if (closeTagIndex == -1) return null;
            var tokenValue = paragraph[position..(closeTagIndex + tag.Length)];
            var temporaryToken = new TemporaryToken(tokenValue, paragraphIndex, position);
            if (identifiers[tag].Identify(paragraphs, temporaryToken, out var identifiedToken))
                openedTags.Add(tag);
            return identifiedToken;
        }

        private Token TryReadLineToken(string[] paragraphs, int paragraphIndex, int position, string tag)
        {
            var temporaryToken = new TemporaryToken(paragraphs[paragraphIndex], paragraphIndex, position);
            if (identifiers[tag].Identify(paragraphs, temporaryToken, out var identifiedToken))
                openedTags.Add(tag);
            return identifiedToken;
        }

        private int IndexOfCloseTag(string paragraph, string openTag, int openTagIndex)
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

        private void ResetTemporaryResources()
        {
            openedTags.Clear();
            plainText.Clear();
        }
    }
}