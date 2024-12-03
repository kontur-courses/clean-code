using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface IParser
    {
        public DOM BuildDom(string text);
    }
}
