using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main1(string[] args)
        {
            Console.WriteLine(Md.Render("_ab__cd_"));
        }
    }
}
