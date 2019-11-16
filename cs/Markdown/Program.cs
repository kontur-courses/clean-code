using System;
using System.IO;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "Hello my __friend__ \nI am __Markdown__ \n\\_I can change the lines like this\\_ -> _I can change the lines like this_ ";

            var md = new Markdown();
            var res = md.Render(input);

            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
