using System;
using System.Collections.Generic;
using Markdown.Md.PairedTagsHandlers;
using Markdown.Md.TagHandlers;

namespace Markdown.Md
{
    public class MdParser : IParser<MdToken>
    {
        private readonly MdTagHandler tagHandler;
        private readonly PairedTagsHandler pairedTagsHandler;
        private readonly MdPairedTagsState pairedTagsState;

        public MdParser()
        {
            pairedTagsState = new MdPairedTagsState();
            tagHandler = new EmphasisHandler().SetSuccessor(new TextHandler());
            pairedTagsHandler = new OpenEmphasisHandler().SetSuccessor(
                new CloseEmphasisHandler().SetSuccessor(
                    new OpenStrongEmphasisHandler().SetSuccessor(new CloseStrongEmphasisHandler())));
        }

        public MdToken[] Parse(string str)
        {
            if (str == null)
            {
                throw new ArgumentException("Given string can't be null", nameof(str));
            }

            var result = new List<MdToken>();

            for (var position = 0; position < str.Length; position++)
            {
                var token = tagHandler.Handle(str, position);
                result.Add(token);
                position += token.Value.Length - 1;
                pairedTagsHandler.Handle(pairedTagsState, result.Count - 1, token);
            }

            ConvertToTextInvalidTags(result);

            return result.ToArray();
        }

        private void ConvertToTextInvalidTags(IReadOnlyList<MdToken> result)
        {
            while (pairedTagsState.InvalidTokens.Count != 0)
            {
                var item = pairedTagsState.InvalidTokens.First.Value;
                pairedTagsState.InvalidTokens.RemoveFirst();
                result[item.Item1]
                    .Type = MdType.Text;
            }

            while (pairedTagsState.OpeningTagsTokens.Count != 0)
            {
                var item = pairedTagsState.OpeningTagsTokens.Pop();
                result[item.Item1]
                    .Type = MdType.Text;
            }
        }
    }
}