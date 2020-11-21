using System.Collections.Generic;
using Markdown.Models.ConvertOptions.ConvertRules;
using Markdown.Models.ConvertOptions.UnionRules;

namespace Markdown.Models.ConvertOptions
{
    internal class ConvertingOptions
    {
        public IEnumerable<IConvertRule> ConvertRules { get; }
        public IEnumerable<IUnionRule> UnionRules { get; }
        public IConvertRule NewLineRule { get; }

        public ConvertingOptions(IEnumerable<ConvertRules.IConvertRule> convertingRules,
            IEnumerable<IUnionRule> unionRules, IConvertRule newLineRule)
        {
            ConvertRules = convertingRules;
            UnionRules = unionRules;
            NewLineRule = newLineRule;
        }
    }
}
