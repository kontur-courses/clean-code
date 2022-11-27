using System;
using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class InlineTagParser : ITagParser
    {
        private readonly TagStorage storage;
        public InlineTagParser(TagStorage storage)
        {
            this.storage = storage;

        }
        public List<TypedToken> Parse(string text, ITag tag)
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
    }
}
