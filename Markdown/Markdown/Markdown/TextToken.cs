using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TextToken : IToken
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public bool HaveSpaces;

        public TextToken(int length,int position)
        {
            HaveSpaces=CheckForSpaces();
            Length = length;
            Position = position;
        }

        public bool CheckForSpaces()
        {
            return false;
        }

        
    }
}
