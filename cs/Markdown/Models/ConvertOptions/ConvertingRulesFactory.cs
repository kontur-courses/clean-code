using System.Collections.Generic;
using Markdown.Models.ConvertOptions.ConvertRules;

namespace Markdown.Models.ConvertOptions
{
    internal static class ConvertingRulesFactory
    {
        public static List<IConvertRule> GetAllRules()
        {
            return new List<IConvertRule>()
            {
                new UnderscoreToEm(),
                new DoubleUnderscoreToStrong(),
                new SharpToH1(),
                new PlusToUnorderedListElement()
            };
        }
    }
}
