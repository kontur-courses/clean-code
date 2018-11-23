using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Window
    {
        public string SourceString { get; }
        public int Position { get; set; }

        public Window(string sourceString, int position)
        {
            SourceString = sourceString;
            Position = position;
        }

        public char this[int i]
        {
            get
            {
                var index = i + Position;
                if (index < 0 || index >= SourceString.Length)
                    return '\0';
                return SourceString[i + Position];
            }
        }
    }
}
