namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var operation = new RenderOperation(text);
            operation.Process();
            return operation.Result;
        }
    }
}