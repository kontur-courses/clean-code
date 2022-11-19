using Markdown.Core.Entities;
using Markdown.Core.Interfaces;

namespace Markdown.Core.Helpers
{
    public class MdParser : IMdParser
    {
        public IEnumerable<TagNode> Parse(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}