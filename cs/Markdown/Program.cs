using System;

namespace Markdown
{
    public class Program
    {
        public static void Main()
        {
            var md = new Md();
            Console.WriteLine(md.Render("_test __test__ test_", new MdParser(), new MdConverter()));
        }
    }
}