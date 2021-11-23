using System.Collections.Generic;

namespace Markdown
{
    // Сам считаю это вообще не очень решением, но пока не знаю, что с ним делать
    public static class Tags
    {
        public static readonly IEnumerable<string> MarkdownTags = new HashSet<string> {"_", "__", "#"};
    }
}