using System.Collections.Generic;

namespace Markdown.Models.Converters
{
    internal interface IConverter
    {
        public string Convert(IEnumerable<ITaggedToken> tokens, bool withNewLine);
    }
}
