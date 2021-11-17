namespace Markdown
{
    public class Md
    {
        public static string Render(string text)
        {
            return text
                .ParseIntoTokens()
                .RenderToHtml();
        }
    }
}