using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Interfaces;

namespace Markdown
{
    public class Tag: ITag
    {
        private string HTMLtag;
        private string tagContent;

        public Tag(string htmLtag, string tagContent)
        {
            throw new NotImplementedException();
        }

        public string WrapTagIntoHtml()
        {
            throw new NotImplementedException();
        }
    }
}
