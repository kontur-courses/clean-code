using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TokenReader
    {
        private readonly TagStorage tagStorage;

        public TokenReader(TagStorage tagStorage)
        {
            this.tagStorage = tagStorage;
        }

        public IReadOnlyList<TypedToken> Read(string text)
        {
            var result = new List<TypedToken>();

            var textControlTokens = new List<TypedToken>();

            textControlTokens.AddRange(GetTagTokens(text));

            textControlTokens.AddRange(GetEscapeTokens(text));

            var textTokens = GetTextTokens(text, textControlTokens.OrderBy(token => token.Start).ToList());

            result.AddRange(textControlTokens);

            result.AddRange(textTokens);

            return result
                .OrderBy(token => token.Start)
                .ToList()
                .RemoveEscapedTags()
                .RemoveUnpairedTags()
                .RemoveTagsWithInvalidContentBetween(text)
                .RemoveTagsWithInvalidNesting(tagStorage);
        }

        private List<TypedToken> GetTagTokens(string text)
        {
            var result = new List<TypedToken>();

            var positionsWithTag = new HashSet<int>();

            foreach (var tag in tagStorage.Tags.OrderByDescending(tag => tag.OpeningSubTag.Length))
            {
                if (tag.OpeningSubTag == "" || tag.ClosingSubTag == "")
                    continue;

                var tagTokens = new List<TypedToken>();

                if (tag.Type == TagType.Header)
                {
                    tagTokens = ParseLineTagTokens(text, tag);
                }
                else if (tag.Type == TagType.Italic || tag.Type == TagType.Strong)
                {
                    tagTokens = ParseInlineTagTokens(text, tag);
                }
                else
                {
                    tagTokens = ParseUnorderedListTagsTokens(text, tag);
                }

                foreach (var tagToken in tagTokens)
                {
                    if (positionsWithTag.Contains(tagToken.Start))
                        continue;

                    for (var j = tagToken.Start; j <= tagToken.End; j++)
                        positionsWithTag.Add(j);

                    if (tag.IsValid(text, tagToken.Order, tagToken.Start))
                        result.Add(tagToken);
                }
            }

            return result;
        }

        private List<TypedToken> ParseUnorderedListTagsTokens(string text, ITag tag)
        {
            if (tagStorage.GetSubTag(TagType.UnorderedList, SubTagOrder.Opening) != "")
                return ParseLineTagTokens(text, tag);

            var listItemTagTokens = ParseLineTagTokens(text, tag);

            if (listItemTagTokens.Count == 0)
                return listItemTagTokens;

            var first = listItemTagTokens[0];

            var last = listItemTagTokens[listItemTagTokens.Count - 1];

            listItemTagTokens.Add(new TypedToken(
                                            first.Start - 1,
                                            1,
                                            TokenType.Tag,
                                            TagType.UnorderedList,
                                            SubTagOrder.Opening));

            listItemTagTokens.Add(new TypedToken(
                                            last.End + 1,
                                            1,
                                            TokenType.Tag,
                                            TagType.UnorderedList,
                                            SubTagOrder.Closing));

            return listItemTagTokens;
        }

        private List<TypedToken> ParseLineTagTokens(string text, ITag tag)
        {
            var tagTokens = new List<TypedToken>();

            var subTagOrder = SubTagOrder.Closing;

            for (var i = 0; i < text.Length;)
            {
                subTagOrder = subTagOrder == SubTagOrder.Closing ? SubTagOrder.Opening : SubTagOrder.Closing;

                var subTag = tag.GetSubTag(subTagOrder);

                var subTagIndex = text.IndexOf(subTag, i, StringComparison.Ordinal);

                if (subTagIndex == -1 && subTagOrder == SubTagOrder.Opening)
                    break;

                if (subTagIndex == -1 && subTagOrder == SubTagOrder.Closing)
                {
                    tagTokens.Add(new TypedToken(tag, subTagOrder, text.Length));
                    break;
                }

                tagTokens.Add(new TypedToken(tag, subTagOrder, subTagIndex));

                i = subTagIndex + subTag.Length;
            }

            return tagTokens;
        }

        private List<TypedToken> ParseInlineTagTokens(string text, ITag tag)
        {
            var tagTokens = new List<TypedToken>();

            var subTagOrder = SubTagOrder.Closing;

            for (var i = 0; i < text.Length;)
            {
                subTagOrder = subTagOrder == SubTagOrder.Closing ? SubTagOrder.Opening : SubTagOrder.Closing;

                var subTag = tag.GetSubTag(subTagOrder);

                var subTagIndex = text.IndexOf(subTag, i, StringComparison.Ordinal);

                if (subTagIndex == -1)
                    break;

                tagTokens.Add(new TypedToken(tag, subTagOrder, subTagIndex));

                i = subTagIndex + subTag.Length;
            }

            return tagTokens;
        }

        private List<TypedToken> GetEscapeTokens(string text)
        {
            var escapeTokens = new List<TypedToken>();

            var escapeCharacter = tagStorage.EscapeCharacter;

            for (var i = 0; i < text.Length;)
            {
                var escapeCharacterIndex = text.IndexOf(escapeCharacter, i, StringComparison.Ordinal);

                if (escapeCharacterIndex == -1)
                    break;

                escapeTokens.Add(new TypedToken(escapeCharacterIndex, escapeCharacter.Length, TokenType.Escape));

                i = escapeCharacterIndex + escapeCharacter.Length;
            }

            return escapeTokens;
        }

        private List<TypedToken> GetTextTokens(string text, List<TypedToken> tagTokens)
        {
            if (!tagTokens.Any())
                return new List<TypedToken> { new TypedToken(0, text.Length, TokenType.Text) };

            var textTokens = new List<TypedToken>();

            textTokens.AddTextFromBeginningUpToTag(tagTokens.First());

            for (var i = 0; i < tagTokens.Count - 1; i++)
                textTokens.AddTextBetween(tagTokens[i], tagTokens[i + 1]);

            var textLength = text.Length - tagTokens.Last().End - 1;

            textTokens.AddTextAfterTag(tagTokens.Last(), textLength);

            return textTokens;
        }
    }
}