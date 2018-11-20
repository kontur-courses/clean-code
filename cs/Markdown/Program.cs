using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;
            var mdInput = args[0];
            var md = new Md();
            var htmlResult = md.Render(mdInput);
            Console.WriteLine(htmlResult);
        }
    }
}