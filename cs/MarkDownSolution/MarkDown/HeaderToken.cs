using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class HeaderToken : Token
    {
        private string type = "header";
        public HeaderToken(int start, int length) : base(start, length)
        {
        }
    }
}
