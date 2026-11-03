using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{

    internal class HtmlRenderer : IHtmlRenderer
    {
        private readonly TagRegistry registry;

        public HtmlRenderer(TagRegistry registry)
        {
            this.registry = registry;
        }

        public string Render(List<Token> tokens)
        {
            throw new NotImplementedException();
        }
        




    }
}
