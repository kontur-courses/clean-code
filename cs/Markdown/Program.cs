using System;

namespace Markdown
{
    public class Program
    {
        public static void Main()
        {
            var md = new Md();
            Console.WriteLine(md.Render("dfdf _dfd_"));
        }
    }
}