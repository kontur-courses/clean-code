using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Window
    {
        private readonly char[] characters = new char[4];

        public char this[int i]
        {
            get => characters[i + 1];
            set => characters[i + 1] = value;
        }
    }
}
