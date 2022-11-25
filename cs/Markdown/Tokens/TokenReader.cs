using System;
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
                var tagTokens = tag.Type == TagType.Header ? ParseHeaderTag(text, tag) : ParseInlineTag(text, tag);

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

        private List<TypedToken> ParseHeaderTag(string text, ITag tag)
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

        private List<TypedToken> ParseInlineTag(string text, ITag tag)
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

        private List<TypedToken> GetEscapeTokens(string line)
        {
            var escapeTokens = new List<TypedToken>();

            var escapeCharacter = tagStorage.EscapeCharacter;

            for (var i = 0; i < line.Length;)
            {
                var escapeCharacterIndex = line.IndexOf(escapeCharacter, i, StringComparison.Ordinal);

                if (escapeCharacterIndex == -1)
                    break;

                escapeTokens.Add(new TypedToken(escapeCharacterIndex, escapeCharacter.Length, TokenType.Escape));

                i = escapeCharacterIndex + escapeCharacter.Length;
            }

            return escapeTokens;
        }

        private List<TypedToken> GetTextTokens(string line, List<TypedToken> tagTokens)
        {
            if (!tagTokens.Any())
                return new List<TypedToken> { new TypedToken(0, line.Length, TokenType.Text) };

            var textTokens = new List<TypedToken>();

            textTokens.AddTextFromBeginningUpToTag(tagTokens.First());

            for (var i = 0; i < tagTokens.Count - 1; i++) textTokens.AddTextBetween(tagTokens[i], tagTokens[i + 1]);

            var textLength = line.Length - tagTokens.Last().End - 1;

            textTokens.AddTextAfterTag(tagTokens.Last(), textLength);

            return textTokens;
        }
    }
}