using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Markdown
{
    class StrongRegister : EmRegister
    {
        public StrongRegister()
        {
            suffixLength = 2;
            suffixes = new string[] { "**", "__" };
            priority = 1;
            tags = new string[] { "<strong>", "</strong>" };
        }
    }
}
