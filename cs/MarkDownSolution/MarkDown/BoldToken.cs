using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class BoldToken : Token
    {
        public string type = "bold";

        public BoldToken(int start) : base(start)
        {
        }

        public BoldToken(int start, int length) : base(start, length)
        {
        }
    }
}
