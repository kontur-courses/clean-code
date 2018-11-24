using System;


namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "_a __abc__";
            var result = Md.Render(str);

            Console.WriteLine(result);
        }
    }
}
