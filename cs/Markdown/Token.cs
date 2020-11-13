using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public string Value { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public List<TokenType> TokenTypes { get; set; }
    }
}
