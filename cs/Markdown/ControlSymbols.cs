using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class ControlSymbols
    {
        public static readonly Dictionary<string, Func<string, int, StopSymbolDecision>> ControlSymbolDecisionOnChar =
            new Dictionary<string, Func<string, int, StopSymbolDecision>>()
            {
                {"_", MakeDecisionForOneUnderscores},
                {"__", MakeDecisionForTwoUnderscores}
            };

        public static readonly HashSet<string> ControlSymbol = new HashSet<string>() {"_", "__"};
        private static readonly HashSet<char> previousStopSymbol = new HashSet<char>() {' ', '\\'};

        public static readonly Dictionary<string, string> ControlSymbolTags = new Dictionary<string, string>()
            {{"_", "em"}, {"__", "strong"}};
        public static readonly Dictionary<string, HashSet<string>> TagCloseNextTag = new Dictionary<string, HashSet<string>>
        {
            {"em", new HashSet<string>{"em", "strong"}},
            {"strong", new HashSet<string>{"strong"}}
        };
        
        public static string ResolveControlSymbol(string input, int position)
        {
            if (input.Substring(position, 2) == "__")
                return "__";
            return "_";
        }

        private static StopSymbolDecision MakeDecisionForOneUnderscores(string input, int position)
        {
            if (input[position] != '_')
                return StopSymbolDecision.Continue;
            if (isNastedOneUnderscores(input, position))
                return StopSymbolDecision.NestedToken;
            return isEndOfOneUnderscores(input, position)
                ? StopSymbolDecision.Stop
                : StopSymbolDecision.Continue;
        }

        private static StopSymbolDecision MakeDecisionForTwoUnderscores(string input, int position)
        {
            if (input[position] != '_')
                return StopSymbolDecision.Continue;

            if (isNastedTwoUnderscores(input, position))
                return StopSymbolDecision.NestedToken;
            return isEndOfDoubleUnderscores(input, position)
                ? StopSymbolDecision.Stop
                : StopSymbolDecision.Continue;
        }

        private static bool isEndOfDoubleUnderscores(string input, int position)
        {
            return (position > 0 && !previousStopSymbol.Contains(input[position - 1])) &&
                   (position + 1 < input.Length && input[position + 1] == '_') &&
                   (position + 2 == input.Length || (position + 2 < input.Length && input[position + 2] == ' '));
        }

        private static bool isNastedTwoUnderscores(string input, int position)
        {
            if (position - 1 < 0 || position + 1 >= input.Length || input[position - 1] == '\\' ||
                input[position - 1] != ' ') return false;
            return input[position + 1] != ' ' && input[position + 1] != '_';
        }

        private static bool isNastedOneUnderscores(string input, int position)
        {
            if (position - 1 < 0 || position + 2 >= input.Length || input[position - 1] == '\\' ||
                input[position - 1] != ' ') return false;
            return input[position + 2] != ' ' && input[position + 1] == '_';
        }
        
        private static bool isEndOfOneUnderscores(string input, int position)
        {
            return position > 0 && !previousStopSymbol.Contains(input[position - 1])
                   && input[position - 1] != '_'
                   && (position + 1 == input.Length || input[position + 1] == ' ');
        }
    }
}