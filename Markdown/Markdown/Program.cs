using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parser;

namespace Markdown
{
    class Program
    {
        public static void Main()
        {
            var md = new MarkDown(new MdTagParser());
            var mdText = "** str **";
        }
    }
}
