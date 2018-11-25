namespace Markdown
{
    public static class Md
    {
       public static string Render(string source)
       {
            var parser = new MarkdownParser(source);          
            var interpreter = new MarkdownInterpreter(parser.Parse());
            return interpreter.GetHtmlCode();
       }
    }
}
