using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Interfaces;

namespace Markdown
{
    class MdTagParser: IParser<Tag>
    {
        private Dictionary<string, string> MdToHtmlMatches;

        public List<Tag> Parse(string textToParse)
        {
            throw new NotImplementedException();
        }
    }
}
