using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Generators
{
    public class HtmlGenerator : IGenerator
    {
        private readonly IDictionary<TagType, string> tags;

        public HtmlGenerator(IDictionary<TagType, string> tags) 
        {
            this.tags = tags;
        }

        public string Generate(IEnumerable<IToken> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
