using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class StringRenderer
    {
        protected readonly Dictionary<string, Tag> Tags;
        private readonly ITokenSelector tokenSelector;

        public StringRenderer(ITokenSelector tokenSelector)
        {
            this.tokenSelector = tokenSelector;
            Tags = new Dictionary<string, Tag>();
        }

        public string Render(string input)
        {
            var tokenReader = new TokenReader(Tags);
            var tokens = tokenReader.ReadTokens(input);
            var tags = tokenSelector.SelectTokens(tokens);
            HandleTags(tags, new List<Tag>());

            return string.Join("", tokens.Select(x => x.Value));
        }

        public void HandleTags(IEnumerable<Token> tags, List<Tag> upperTags)
        {
            var OpenTag = tags.FirstOrDefault(token => token.IsOpen);
            if (OpenTag == null)
                return;

            var CloseTag = tags.FirstOrDefault(token =>
                token.PosibleTag == OpenTag.PosibleTag && token.IsClose && token != OpenTag);
            if (CloseTag == null)
            {
                HandleTags(tags.SkipWhile(token => token != OpenTag).Skip(1), upperTags);
            }
            else
            {
                upperTags.Add(OpenTag.PosibleTag);
                HandleTags(
                    tags.SkipWhile(t => t != OpenTag).Skip(1).Reverse().SkipWhile(t => t != CloseTag).Skip(1).Reverse(),
                    upperTags);
                upperTags.RemoveRange(upperTags.Count - 1, 1);
                HandleTags(tags.SkipWhile(t => t != CloseTag).Skip(1), upperTags);
                if (OpenTag.PosibleTag.IsValidTag(upperTags))
                {
                    OpenTag.Value = "<" + OpenTag.PosibleTag.HtmlRepresentation + ">";
                    CloseTag.Value = "</" + CloseTag.PosibleTag.HtmlRepresentation + ">";
                }
            }
        }
    }
}