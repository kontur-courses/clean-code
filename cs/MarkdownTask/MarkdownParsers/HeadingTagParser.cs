using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public class HeadingTagParser : ITagParser
    {
        private string mdTag = "# ";
        private string htmlTag = "h1";
        public ICollection<Token> ParseTag(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}
