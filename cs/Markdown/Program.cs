using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();
            var f = md.Render(@"#_\ a_");
            Console.WriteLine(f);
            while (true)
            {
                var str = Console.ReadLine();
                var a = md.Render(str);
                Console.WriteLine(a);
            }
        }
    }
}
