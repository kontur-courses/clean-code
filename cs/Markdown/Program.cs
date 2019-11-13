using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "__А  _ФФ Ф__ Ф_";

            var md = new Markdown();
            var res = md.Render(input);

            Console.WriteLine(input);
            Console.WriteLine();
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
