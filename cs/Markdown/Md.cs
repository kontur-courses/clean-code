using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            return text;
        }
    }
}
