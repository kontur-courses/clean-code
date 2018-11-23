using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class StringRenderer
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

        public abstract void HandleTags(IEnumerable<Token> tags, List<Tag> upperTags);

    }
 }