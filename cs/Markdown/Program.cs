namespace Markdown
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();

            var result = md.Render("#Hello, world!");
            Console.WriteLine(result);

            Console.WriteLine();

            result = md.Render("_Hello, world!_");
            Console.WriteLine(result);

            Console.WriteLine();

            result = md.Render("__Hello, world!__");
            Console.WriteLine(result);
        }
    }
}
