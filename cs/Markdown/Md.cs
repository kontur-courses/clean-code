using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly IMdSpecification _specification;

        public Md(IMdSpecification specification)
        {
            _specification = specification;
        }

        public string Render(string mdText)
        {
            var parser = new MarkdownParser(_specification);
            var tokens = parser.ParseToTokens(mdText);
            var renderedTokens = tokens.Select(t => t.Render(mdText));
            return string.Join("", renderedTokens);
        }
    }
}
