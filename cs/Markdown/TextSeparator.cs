using System.Collections.Generic;

namespace Markdown
{
    public struct TextSeparator
    {
        public string Separator { get; }
        public LinkedListNode<string> Index { get; }

        public TextSeparator(string separator, LinkedListNode<string> index)
        {
            Separator = separator;
            Index = index;
        }
    }
}
