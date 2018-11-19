using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Program
    {
        private static void Main()
        {
            var types = new List<MdType>
            {
                MdType.SingleUnderLine,
                MdType.DoubleUnderLine,
                MdType.Sharp,
                MdType.TripleGraveAccent
            };
            var md = new Md(types);
            Console.WriteLine(md.Render("_a __b__ c_"));
        }
    }
}