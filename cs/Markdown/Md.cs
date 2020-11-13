namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdown) => new MdTokenReader(markdown).ReadAndParseAll();
    }
}