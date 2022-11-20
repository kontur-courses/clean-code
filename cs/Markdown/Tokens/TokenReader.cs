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

        public IReadOnlyList<Token> Read(string inputText)
        {
            var result = new List<Token>();

            var tagTokens = GetTagTokens(inputText);

            var textTokens = GetTextTokens(inputText, tagTokens);

            result.AddRange(tagTokens);

            result.AddRange(textTokens);

            return result.OrderBy(token => token.Start).ToList();
        }

        private List<TagToken> GetTagTokens(string inputText)
        {
            var result = new List<TagToken>();

            foreach (var tag in tagStorage.Tags)
            {
                var tagTokens = GetTagTokens(inputText, tag);

                result.AddRange(tagTokens);
            }

            return result.OrderBy(token => token.Start).ToList();
        }

        private List<TagToken> GetTagTokens(string line, ITag tag)
        {
            var tagTokens = new List<TagToken>();

            var subTagSequence = new Stack<SubTagOrder>();

            for (var i = 0; i < line.Length;)
            {
                if (subTagSequence.Count == 0 || subTagSequence.Peek() == SubTagOrder.Closing)
                    subTagSequence.Push(SubTagOrder.Opening);
                else
                    subTagSequence.Push(SubTagOrder.Closing);

                var subTag = tag.GetSubTag(subTagSequence.Peek());

                var subTagIndex = line.IndexOf(subTag, i, StringComparison.Ordinal);

                if (subTagIndex == -1)
                    break;

                tagTokens.Add(new TagToken(TokenType.Tag, subTagIndex, subTag.Length, subTagSequence.Peek()));

                i = subTagIndex + subTag.Length;
            }

            return tagTokens;
        }
         
        private List<Token> GetTextTokens(string line, List<TagToken> tagTokens)
        {
            var textTokens = new List<Token>();

            textTokens.AddTextFromBeginningUpToTag(tagTokens.First());

            for (var i = 0; i < tagTokens.Count - 1; i++)
            {
                textTokens.AddTextBetween(tagTokens[i], tagTokens[i + 1]);
            }

            var textLength = line.Length - tagTokens.Last().End - 1;

            textTokens.AddTextAfterTag(tagTokens.Last(), textLength);

            return textTokens.OrderBy(token => token.Start).ToList();
        }

    }
}
