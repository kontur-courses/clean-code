using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class Block(List<Token> tokens)
    {
        public List<Token> tokens = tokens;
    }
}
