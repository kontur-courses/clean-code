using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MdTag;
using Markdown.Parser;

namespace Markdown
{
    public class MarkDown
    {
        private MdTagParser parser;

        public string Render(string rawMarkdown)
        {
            throw new NotImplementedException();
        }

        public MarkDown(IParser<MdTag.Tag> parser)
        {
            var a = new Tag("qw", "dwq");
            this.parser = new MdTagParser();
        }

        private string CombineTagsInOneString(List<MdTag.Tag> tags)
        {
            throw new NotImplementedException();
        }
    }
}
