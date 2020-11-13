using System;

namespace Markdown
{
    public class Renderer
    {
        public static string Render(string textInMarkdown)
        {
            if (string.IsNullOrEmpty(textInMarkdown))
                throw new ArgumentException("String is null or empty");
            return "";
        }
    }
}