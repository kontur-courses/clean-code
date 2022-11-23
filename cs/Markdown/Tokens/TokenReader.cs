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

        public IReadOnlyList<TypedToken> Read(string inputText)
        {
            var result = new List<TypedToken>();

            var tagTokens = GetTagTokens(inputText);

            var escapeTokens = GetEscapeTokens(inputText);

            var textControlTokens = new List<TypedToken>();

            textControlTokens.AddRange(tagTokens);

            textControlTokens.AddRange(escapeTokens);

            var textTokens = GetTextTokens(inputText, textControlTokens.OrderBy(token => token.Start).ToList());

            result.AddRange(textControlTokens);

            result.AddRange(textTokens);

            return result.OrderBy(token => token.Start).ToList().RemoveEscaped().RemoveUnpaired();

            //TODO
            //Разобрать кучу коллекций тегов
            // Переделать тесты, так как непарные теги будут отдельным текстовым токеном
            // Переделать коллекции на IEnumerable;
        }


        private List<TypedToken> GetTagTokens(string inputText)
        {
            var result = new List<TypedToken>();

            var positionsWithTag = new HashSet<int>();

            foreach (var tag in tagStorage.Tags.OrderByDescending(tag => tag.OpeningSubTag.Length))
            {
                var tagTokens = GetTagTokensForSpecificTag(inputText, tag);

                foreach (var tagToken in tagTokens)
                {
                    if (positionsWithTag.Contains(tagToken.Start))
                        continue;

                    for (var j = tagToken.Start; j <= tagToken.End; j++)
                        positionsWithTag.Add(j);

                    result.Add(tagToken);
                }
            }

            return result;
        }

        private List<TypedToken> GetTagTokensForSpecificTag(string line, ITag tag)
        {
            var tagTokens = new List<TypedToken>();

            var isOpeningTag = true;

            for (var i = 0; i < line.Length;)
            {
                var subTagOrder = isOpeningTag ? SubTagOrder.Opening : SubTagOrder.Closing;

                var subTag = tag.GetSubTag(subTagOrder);

                var subTagIndex = line.IndexOf(subTag, i, StringComparison.Ordinal);

                if (subTagIndex == -1)
                    break;

                tagTokens.Add(new TypedToken(subTagIndex, subTag.Length, TokenType.Tag, tag.Type, subTagOrder));

                i = subTagIndex + subTag.Length;

                isOpeningTag = !isOpeningTag;
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

            for (var i = 0; i < tagTokens.Count - 1; i++)
            {
                textTokens.AddTextBetween(tagTokens[i], tagTokens[i + 1]);
            }

            var textLength = line.Length - tagTokens.Last().End - 1;

            textTokens.AddTextAfterTag(tagTokens.Last(), textLength);

            return textTokens;
        }

    }
}
