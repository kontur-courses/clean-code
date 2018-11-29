using System;

namespace Markdown
{
    public static class Markdown
    {
        public static string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var tagReader = new TagReader(text, new StrongTagInfo(), new EmTagInfo());
            return tagReader.Evaluate();
        }
    }
}
