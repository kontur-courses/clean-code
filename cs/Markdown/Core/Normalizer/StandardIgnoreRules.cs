using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Infrastructure;

namespace Markdown.Core.Normalizer
{
    public class StandardIgnoreRules
    {
        private static readonly List<string> OuterTags = new List<string>() {"em"};
        private const string InnerTag = "strong";

        public static List<IgnoreInsideRule> IgnoreInsideRules = new List<IgnoreInsideRule>()
        {
            new IgnoreInsideRule(
                OuterTags.Select(TagsUtils.GetTagInfoByTagName).ToList(),
                TagsUtils.GetTagInfoByTagName(InnerTag))
        };
    }
}