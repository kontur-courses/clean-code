using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class ItalicToken : Token
    {
        private string type = "italic";
        public ItalicToken(int start, int length) : base(start, length)
        {
        }
    }
}
