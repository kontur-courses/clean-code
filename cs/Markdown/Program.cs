namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md(new ParsersHandler());
            md.Render("_Hello world_");
        }
    }
}