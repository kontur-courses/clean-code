using Markdown.MdParsing;

namespace Markdown
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("# __Заголовок__ с _разными_ символами__"));
        }
    }
}
