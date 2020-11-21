using System.Collections.Generic;
using Markdown.Models.ConvertOptions.ConvertRules;

namespace Markdown.Models.Converters
{
    internal interface IConverter
    {
        public IConvertRule NewLineRule { get; }
        public string Convert(IEnumerable<IEnumerable<ITaggedToken>> textTokens);
    }
}
