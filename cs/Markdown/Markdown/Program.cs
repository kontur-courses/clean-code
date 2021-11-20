using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = Md.Render("\\a");
            Console.WriteLine(res);
        }
    }
}
