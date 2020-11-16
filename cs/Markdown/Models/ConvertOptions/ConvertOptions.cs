using System.Collections.Generic;
using Markdown.Models.ConvertOptions.ConvertRules;
using Markdown.Models.ConvertOptions.UnionRules;
using Markdown.Models.Tags;

namespace Markdown.Models.ConvertOptions
{
    internal class ConvertingOptions
    {
        public IEnumerable<IConvertRule> ConvertRules { get; }
        public IEnumerable<IUnionRule> UnionRules { get; }
        public Tag NewLine { get; }

        public ConvertingOptions(IEnumerable<IConvertRule> convertingRules,
            IEnumerable<IUnionRule> unionRules, Tag newLineTag)
        {
            ConvertRules = convertingRules;
            UnionRules = unionRules;
            NewLine = newLineTag;
        }
    }
}
