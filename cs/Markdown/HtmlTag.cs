namespace Markdown
{
    public static class HtmlTag
    {
        public static string Open(string name)
        {
            return $"<{name}>";
        }

        public static string Close(string name)
        {
            return $"</{name}>";
        }

        public static string Wrap(string name, string content)
        {
            return Open(name) +
                   content +
                   Close(name);
        }
    }
}
