using System.Collections.Generic;

namespace Markdown
{
    public static class HtmlConverter
    {
        public static readonly Dictionary<AttributeType, string> TagDictionary;

        static HtmlConverter()
        {
            TagDictionary = new Dictionary<AttributeType, string>
            {
                {AttributeType.Emphasis, "em"},
                {AttributeType.Strong, "strong"},
            };
        }
    }
}