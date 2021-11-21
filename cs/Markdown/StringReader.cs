using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StringReader
    {
        private string _text;
        private int _pointer;

        public StringReader(string text)
        {
            _text = text;
            _pointer = 0;
        }

        public bool MoveNext()
        {
            if (HasNext())
            {
                _pointer++;
                return true;
            }
            return false;
        }

        public bool HasNext() => _pointer != _text.Length - 1;
    }
}
