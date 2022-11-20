using System;
using System.Text;

namespace Markdown
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var text = @"# Заголовок __с _разными_ символами__";
            Console.WriteLine(text);
            var a = Md.Render(text);
            Console.WriteLine(a);
            // StringBuilder builder = new StringBuilder();
            // builder.Append("Еще _одна пр_оверка");
            // builder.Replace("_", "#", 0, 7);
            // Console.WriteLine(builder);
        }
    }
}
