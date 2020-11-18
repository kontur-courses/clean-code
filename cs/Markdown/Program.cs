using System;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md =  new Md();
            var f = md.Render("#hello world");
            Console.WriteLine(f);
        }
    }
}
