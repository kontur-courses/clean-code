using System;

namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md();
            var mdText = @"_a__a_";
            var htmlText = md.RenderToHtml(mdText);
            Console.WriteLine($"md   : {mdText}\nhtml : {htmlText}");
        }
    }
}
