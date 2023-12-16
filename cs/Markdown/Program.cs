namespace Markdown
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("# Заголовок __с _разными_ символами__"));
        }
    }
}
