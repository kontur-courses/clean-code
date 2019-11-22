using System.Collections.Generic;
using System.Linq;
using Markdown.TagsLibrary;

namespace Markdown.Tokens
{
    public class MarkdownTokenizer
    {
        public IEnumerable<Token> SplitTextToTokens(string text)
        {
            var tokens = new List<Token>();
            var tagsCandidates = new List<TagElement>();

            var textTokenLength = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (IsEscapedSymbols(text, i))
                {
                    CreateTextTokenFromAccumulatedLength(text, ref textTokenLength, tokens, i);

                    tokens.Add(CreateTextToken(text, i + 1, 1));
                    i++;
                    continue;
                }

                if (MarkdownTagsLibrary.TryToGetUsableTagInAssociations(text, i, out var element))
                {
                    CreateTextTokenFromAccumulatedLength(text, ref textTokenLength, tokens, i);

                    TryCreateAndAddTokenToList(text, element, tagsCandidates, tokens);

                    i += element.Length - 1;
                    continue;;
                }

                textTokenLength++;
            }

            if (tagsCandidates.Count != 0)
            {
                tokens.AddRange(tagsCandidates
                    .Select(candidate => CreateTextTokenFromUnusedTag(candidate, text)));
                tokens = tokens.OrderBy(x => x.EndIndex).ToList();
            }

            if (textTokenLength != 0)
                tokens.Add(CreateTextToken(text, text.Length - textTokenLength,
                    textTokenLength));

            return tokens;
        }

        private bool IsEscapedSymbols(string text, int index)
        {
            var currentChar = text.TryGetChar(index);
            var nextChar = text.TryGetChar(index + 1);

            return (currentChar == '\\' && nextChar.HasValue &&
                    (nextChar == '\\' || MarkdownTagsLibrary.TagAssociations.ContainsKey(nextChar.ToString())));
        }

        private void TryCreateAndAddTokenToList(string text, TagElement element, List<TagElement> tagsCandidates, List<Token> tokens)
        {
            if (element.TagUsability == TagUsability.Start)
                tagsCandidates.Add(element);
            else
            {
                var token = TryCreateUsableToken(element, tagsCandidates, text);

                if (token == null)
                {
                    if (element.TagUsability == TagUsability.All)
                        tagsCandidates.Add(element);
                    else
                        tokens.Add(CreateTextTokenFromUnusedTag(element, text));
                }
                else
                {
                    DeleteOtherNestedTokens(tokens, token);
                    tokens.Add(token);
                }
            }
        }

        private void CreateTextTokenFromAccumulatedLength(string text, ref int textTokenLength, List<Token> tokens, int i)
        {
            if (textTokenLength == 0) return;

            tokens.Add(CreateTextToken(text, i - textTokenLength,
                textTokenLength));
            textTokenLength = 0;
        }

        private Token TryCreateUsableToken(TagElement endTag, List<TagElement> startTags, string text)
        {
            if (startTags.Count == 0)
                return null;

            var candidateIndex = startTags.Count - 1;
            while (startTags[candidateIndex].Type != endTag.Type)
            {
                if (candidateIndex == 0)
                    return null;
                candidateIndex--;
            }

            var startTag = startTags[candidateIndex];
            if (startTag.EndIndex + 1 == endTag.StartIndex)
                return null;

            startTags.RemoveRange(candidateIndex, startTags.Count - candidateIndex);
            return CreateUsableToken(text, startTag, endTag);
        }

        private Token CreateTextTokenFromUnusedTag(TagElement tag, string text)
        {
            return CreateTextToken(text, tag.StartIndex, tag.Length);
        }

        private Token CreateUsableToken(string text, TagElement startTag, TagElement endTag)
        {
            var startSubstrIndex = startTag.EndIndex + 1;
            var substrLength = endTag.StartIndex - startTag.EndIndex - 1;

            var tagText = text.Substring(startSubstrIndex, substrLength);

            return new Token(tagText, endTag.Type, startTag.StartIndex, endTag.EndIndex);
        }

        private Token CreateTextToken(string text, int tokenStart, int tokenLength)
        {
            var tokenText = text.Substring(tokenStart, tokenLength);

            return new Token(tokenText, TagType.None,
                tokenStart, tokenStart + tokenLength - 1);
        }

        private static void DeleteOtherNestedTokens(List<Token> tokens, Token token)
        {
            var types = MarkdownTagsLibrary.TagTypesToDeleteInRangeOtherTag[token.Type];
            for (var i = tokens.Count - 1; i >= 0; i--)
            {
                if (types.Contains(tokens[i].Type) && tokens[i].EndIndex > token.StartIndex)
                    tokens.RemoveAt(i);
                else
                    break;
            }
        }
    }
}