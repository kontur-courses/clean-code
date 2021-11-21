using System.Collections.Generic;

namespace Markdown.Extensions
{
    public static class CharExtensions
    {
        private static readonly HashSet<char> charset = new HashSet<char>() {'#', '_', '\\'};

        public static bool IsTagSymbol(this char s)
            => charset.Contains(s);
    }
}
