using System.Collections.Generic;

namespace Markdown.Models
{
    public class Group<TKey, TValue>
    {
        public TKey Key { get; }
        public List<TValue> Values { get; } = new();

        public Group(TKey key)
        {
            Key = key;
        }
    }
}