using System;
using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class TagParser : ITagParser
    {
        private readonly TagStorage storage;

        public TagParser(TagStorage storage)
        {
            this.storage = storage;
        }

        public List<TypedToken> Parse(string text, ITag tag)
        {
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

            return tagTokens;
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

        private List<TypedToken> ParseUnorderedListTagsTokens(string text, ITag tag)
        {
            if (storage.GetSubTag(TagType.UnorderedList, SubTagOrder.Opening) != "")
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
    }
}
