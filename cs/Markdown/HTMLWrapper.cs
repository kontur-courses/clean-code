namespace Markdown
{
    public class HTMLWrapper
    {
        public static string WrapWithTag(string tag, string text)
        {
            return $"<{tag}>{text}</{tag}>";
        } 
    }
}