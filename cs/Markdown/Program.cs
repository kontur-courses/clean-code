using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Incorrect input: empty data");
                return;
            }

            var md = new Md();

            var input = Console.In.ReadToEnd();
            var res = md.Render(input);
            Console.Write(res);
        }
    }
}
