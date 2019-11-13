using System.Collections.Generic;

namespace Markdown
{
    public static class HtmlConverter
    {
        public static readonly Dictionary<AttributeType, string> TagDictionary = new Dictionary<AttributeType, string>
        {
            {AttributeType.Emphasis, "em"},
            {AttributeType.Strong, "strong"}
        };
    }
}