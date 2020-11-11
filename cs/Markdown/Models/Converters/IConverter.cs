using System.Collections.Generic;

namespace Markdown.Models.Converters
{
    internal interface IConverter
    {
        public string ConvertMany(IEnumerable<TaggedToken> tokens);
    }
}
