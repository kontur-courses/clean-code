using System.Collections.Generic;

namespace Markdown.Models.ConvertingRules
{
    internal static class ConvertingRulesFactory
    {
        public static IEnumerable<IConvertRule> GetAllRules()
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
