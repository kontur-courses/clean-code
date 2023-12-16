using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public class ItalicTagParser : ITagParser
    {
        private string mdTag = "_";
        private string htmlTag = "em";
        public ICollection<Token> ParseTag(string markdown)
        {
            throw new NotImplementedException();
        }
    }


}
