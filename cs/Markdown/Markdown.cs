using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            for (int i = 0; i < markdown.Length; i++)
            {
                var isSuccess = TagGenerator.TryCreateTag(markdown, i, out string tag);
                if (!isSuccess)
                    continue;
            }

            return string.Empty;
        }
    }
}