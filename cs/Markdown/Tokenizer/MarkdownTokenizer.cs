using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.TagsLibrary;

namespace Markdown.Tokens
{
    public class MarkdownTokenizer
    {
        public IEnumerable<Token> SplitTextToTokens(string text)
        {
            var tokens = new List<Token>();
            var tagsCandidates = new List<TagElement>();
            var shieldingSymbolsIndex = new List<int>();

            var textTokenLength = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\' && i + 1 != text.Length &&
                    (text[i + 1] == '\\' ||
                     MarkdownTagsLibrary.TagAssociations.ContainsKey(text[i + 1].ToString())))
                {
                    if (textTokenLength != 0)
                    {
                        tokens.Add(CreateTextToken(text, i - textTokenLength,
                            textTokenLength, shieldingSymbolsIndex));
                        textTokenLength = 0;
                    }

                    tokens.Add(CreateTextToken(text, i + 1, 1, null));
                    i++;
                    continue;
                }

                if (MarkdownTagsLibrary.TryToGetUsableTagInAssociations(text, i, out var element))
                {
                    if (textTokenLength != 0)
                    {
                        tokens.Add(CreateTextToken(text, i - textTokenLength,
                            textTokenLength, shieldingSymbolsIndex));
                        textTokenLength = 0;
                    }

                    if (element.TagUsability != TagUsability.Start)
                    {
                        var token = TryCreateUsableToken(element, tagsCandidates, text, shieldingSymbolsIndex);

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
                    else
                        tagsCandidates.Add(element);

                    i += element.Length - 1;
                }
                else
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
                    textTokenLength, shieldingSymbolsIndex));

            return tokens;
        }

        private Token TryCreateUsableToken(TagElement endTag, List<TagElement> startTags, string text,
            List<int> shieldingSymbols)
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
            return CreateUsableToken(text, startTag, endTag, shieldingSymbols);
        }

        private Token CreateTextTokenFromUnusedTag(TagElement tag, string text)
        {
            return CreateTextToken(text, tag.StartIndex,
                tag.Length, null);
        }

        private Token CreateUsableToken(string text, TagElement startTag, TagElement endTag, List<int> shieldingSymbols)
        {
            var startSubstrIndex = startTag.EndIndex + 1;
            var substrLength = endTag.StartIndex - startTag.EndIndex - 1;

            var tagText = DeleteAllShieldingSymbols(shieldingSymbols,
                text.Substring(startSubstrIndex, substrLength), startSubstrIndex);

            return new Token(tagText, endTag.Type, startTag.StartIndex, endTag.EndIndex);
        }

        private Token CreateTextToken(string text, int tokenStart, int tokenLength, List<int> shieldingSymbolsIndex)
        {
            var tokenText = DeleteAllShieldingSymbols(shieldingSymbolsIndex,
                text.Substring(tokenStart, tokenLength), tokenStart);

            return new Token(tokenText, TagType.None,
                tokenStart, tokenStart + tokenLength - 1);
        }

        private string DeleteAllShieldingSymbols(List<int> shieldsIndexes, string text, int startIndex)
        {
            if (shieldsIndexes == null || shieldsIndexes.Count == 0)
                return text;

            var tempString = new StringBuilder(text);
            for (var i = 0; i < shieldsIndexes.Count; i++)
                tempString.Remove(shieldsIndexes[i] - i - startIndex, 1);
            shieldsIndexes.Clear();
            return tempString.ToString();
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