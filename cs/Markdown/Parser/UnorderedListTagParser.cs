using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class UnorderedListTagParser : ITagParser
    {
        private readonly TagStorage storage;

        public UnorderedListTagParser(TagStorage storage)
        {
            this.storage = storage;
        }

        public List<TypedToken> Parse(string text, ITag tag)
        {
            if (storage.GetSubTag(TagType.UnorderedList, SubTagOrder.Opening) != "")
                return new LineTagParser(storage).Parse(text, tag);

            var listItemTagTokens = new LineTagParser(storage).Parse(text, tag);

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
