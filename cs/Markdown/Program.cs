using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Readers;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "_\\_a_";
            var readers = new List<IReader> {new SlashReader(), new StrongReader(), new EmReader(),  new CharReader()};
            var md = new Md(readers);
            Console.WriteLine(md.Render(input));
            Console.Read();
        }
    }
}
