using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parser;
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
                .SwitchToTextUnpairedTags()
                .SwitchToTextTagsWithInvalidContentBetween(text)
                .SwitchTextTagsWithInvalidNesting(tagStorage);
        }

        private List<TypedToken> GetTagTokens(string text)
        {
            var result = new List<TypedToken>();

            var positionsWithTag = new HashSet<int>();

            foreach (var tag in tagStorage.Tags.OrderByDescending(tag => tag.OpeningSubTag.Length))
            {
                if (tag.OpeningSubTag == "" || tag.ClosingSubTag == "")
                    continue;

                var tagTokens = ParseTag(text, tag);

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

        private List<TypedToken> ParseTag(string text, ITag tag)
        {
            switch (tag.Type)
            {
                case TagType.Header:
                    return new LineTagParser(tagStorage).Parse(text, tag);
                case TagType.Italic:
                case TagType.Strong:
                    return new InlineTagParser(tagStorage).Parse(text, tag);
                case TagType.UnorderedList:
                case TagType.UnorderedListItem:
                    return new UnorderedListTagParser(tagStorage).Parse(text, tag);
                default:
                    return new List<TypedToken>();
            }
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