using Markdown.MdParsing;

namespace Markdown
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("__ффф _ф ф__ ф __s"));
        }
    }
}
