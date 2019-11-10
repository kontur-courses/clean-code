using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Interfaces;

namespace Markdown
{
    public class MarkDown
    {
        private IParser<Tag> parser;

        public string Render(string rawMarkdown)
        {
            throw new NotImplementedException();
        }

        public MarkDown(IParser<Tag> parser)
        {
            this.parser = new MdTagParser();
        }

        private string CombineTagsInOneString(List<Tag> tags)
        {
            throw new NotImplementedException();
        }
    }
}
