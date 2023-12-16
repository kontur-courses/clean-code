using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public class StrongTagParser : ITagParser
    {
        private string mdTag = "__";
        private string htmlTag = "strong";
        public ICollection<Token> ParseTag(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}
