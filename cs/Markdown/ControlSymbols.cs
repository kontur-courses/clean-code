using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class ControlSymbols
    {
        public static readonly Dictionary<string, Func<string, int, StopSymbolDecision>> controlSymbolDecisionOnChar =
            new Dictionary<string, Func<string, int, StopSymbolDecision>>()
            {
                {"_", MakeDecisionForOneUnderscores},
                {"__", MakeDecisionForTwoUnderscores}
            };

        public static readonly List<string> controlSymbol = new List<string>() {"_", "__"};

        public static readonly Dictionary<string, string> controlSymbolTags = new Dictionary<string, string>()
            {{"_", "em"}, {"__", "strong"}};

        public static string ResolveControlSymbol(string input, int position)
        {
            throw new NotImplementedException();
        }

        private static StopSymbolDecision MakeDecisionForOneUnderscores(string input, int position)
        {
            throw new NotImplementedException();
        }

        private static StopSymbolDecision MakeDecisionForTwoUnderscores(string input, int position)
        {
            throw new NotImplementedException();
        }
    }
}