using System;

namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = "\n- s b\n ```long code``` `code`  `code`  \n# header one\n \n> MH __M__ HM\nHHM _s_ MHMH\n\n";
            Console.WriteLine(input);
            Console.WriteLine(new Md().Renderer(input));
        }
    }
}