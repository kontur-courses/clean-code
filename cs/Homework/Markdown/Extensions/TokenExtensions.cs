namespace Markdown
{
    public static class TokenExtensions
    {
        public static string RenderToHtml(this Token[] tokens)
        {
            return new HtmlRenderer().Render(tokens);
        }
    }
}