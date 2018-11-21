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
                MdType.TripleGraveAccent,
                MdType.Link
            };
            var md = new Md(types);
            Console.WriteLine(md.Render("[gaga](haha)"));
        }
    }
}