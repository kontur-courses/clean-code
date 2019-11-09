using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public string Content;
        public readonly int Index;
        public int Length { get { return Content.Length; } }

        public Token(int index, string content)
        {
            throw new NotImplementedException();
        }
    }
}
