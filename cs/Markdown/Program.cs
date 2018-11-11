using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();

            var value = "fd _ffsdfd_ fsdfs";
            Console.WriteLine($"value :{value}");
            Console.WriteLine(md.Render(value));
        }
    }
}
