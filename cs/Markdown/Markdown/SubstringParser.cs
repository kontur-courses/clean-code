using System.Collections.Generic;


namespace Markdown
{
    class SubstringParser
    {
        private List<StringPart> _result;
        private StringPart _lastStringPart;
        public readonly char EscapeSymbol;

        public SubstringParser(char escapeSymbol = '/')
        {
            EscapeSymbol = escapeSymbol;
            _result = new List<StringPart>();
            _lastStringPart = null;
        }

        public List<StringPart> ParseSubString(string subString)
        {
            _result = new List<StringPart>();
            _lastStringPart = null;

            if (subString.Length == 0 || char.IsWhiteSpace(subString[0]) || StrContainsSymbolsAndDigits(subString))
                return new List<StringPart>() { new StringPart(subString) };

            subString = AddToResultFirstAndLastItems(subString);

            var tmp = ParseSubStringBody(subString, EscapeSymbol);
            _result.AddRange(tmp);

            if (_lastStringPart != null)
                _result.Add(_lastStringPart);

            return _result;
        }

        public static bool StrContainsSymbolsAndDigits(string str)
        {
            if (str == null)
                return false;

            var containSymbols = false;
            var containsDigits = false;

            foreach (var symbol in str)
                if (!char.IsWhiteSpace(symbol))
                {
                    if (IsDigit(symbol))
                        containsDigits = true;
                    else
                        containSymbols = true;
                }

            return containSymbols && containsDigits;
        }

        public static bool IsDigit(char symbol)
        {
            return char.IsDigit(symbol);
        }

        public string AddToResultFirstAndLastItems(string subString)
        {
            if (StartsWithDoubleUnderscore(subString, EscapeSymbol))
            {
                _result.Add(new StringPart("__", ActionType.Open, TagType.Strong));
                subString = subString.Substring(2);
            }
            else if (StartsWithSingleUnderscore(subString, EscapeSymbol))
            {
                _result.Add(new StringPart("_", ActionType.Open, TagType.Em));
                subString = subString.Substring(1);
            }

            if (EndsWithDoubleUnderscore(subString, EscapeSymbol))
            {
                _lastStringPart = new StringPart("__", ActionType.Close, TagType.Strong);
                subString = subString.Substring(0, subString.Length - 2);
            }
            else if (EndsWithSingleUnderscore(subString, EscapeSymbol))
            {
                _lastStringPart = new StringPart("_", ActionType.Close, TagType.Em);
                subString = subString.Substring(0, subString.Length - 1);
            }

            return subString;
        }

        public static List<StringPart> ParseSubStringBody(string str, char escapeSymbol)
        {
            var result = new List<StringPart>();
            var length = str.Length;
            for (var i = 0; i < length; i++)
            {
                var nextSpecialIndex = FindNextSpecialSymbolIndex(str, i, '_');

                if (i != nextSpecialIndex)
                    result.Add(new StringPart(
                        str.Substring(i, nextSpecialIndex - i).ClearFromSymbol(escapeSymbol)));

                if (nextSpecialIndex == length)
                    break;

                i = nextSpecialIndex;

                if (i + 1 < length && str[i + 1] == '_') // case _ _ 
                {
                    if (i + 2 < length)
                    {
                        if (str[i + 2] == '_')
                            result.Add(new StringPart("_"));
                        else
                        {
                            result.Add(new StringPart("__", ActionType.OpenOrClose, TagType.Strong));
                            i++;
                        }
                    }
                    else
                    {
                        result.Add(new StringPart("__"));
                        break;
                    }
                }
                else
                    result.Add(i + 1 < length
                        ? new StringPart("_", ActionType.OpenOrClose, TagType.Em)
                        : new StringPart("_"));
            }

            return result;
        }

        public static int FindNextSpecialSymbolIndex
            (string subString, int currentIndex, char specialSymbol, char escapeSymbol = '/')
        {
            for (; currentIndex < subString.Length; currentIndex++)
            {
                var isPreviousSymbolIsNotEscapeSymbol = currentIndex < 1 || subString[currentIndex - 1] != escapeSymbol;
                if (subString[currentIndex] == specialSymbol && isPreviousSymbolIsNotEscapeSymbol)
                    return currentIndex;
            }

            return subString.Length;
        }

        public static bool StartsWithDoubleUnderscore(string str, char escapeSymbol)
        {
            if (str == null)
                return false;

            var hasSymbolsExceptUnderscore =
                str.Length > 2 && str[2] != '_' && (str.Length == 3 && str[2] != escapeSymbol || str.Length > 3);

            return str.StartsWith("__") && hasSymbolsExceptUnderscore;
        }

        public static bool StartsWithSingleUnderscore(string str, char escapeSymbol)
        {
            if (str == null)
                return false;

            var hasSymbolsExceptUnderscore =
                str.Length > 1 && str[1] != '_' && (str.Length == 2 && str[1] != escapeSymbol || str.Length > 2);

            return str.StartsWith("_") && hasSymbolsExceptUnderscore;
        }

        public static bool EndsWithDoubleUnderscore(string str, char escapeSymbol)
        {
            if (str == null)
                return false;

            var prePenultimateSymbolIsNotSpecial =
                str.Length > 2 && str[str.Length - 3] != '_' || str.Length > 3 && str[str.Length - 4] == escapeSymbol;

            var hasSymbolsExceptUnderscore =
                prePenultimateSymbolIsNotSpecial && (str.Length == 3 && str[0] != escapeSymbol || str.Length > 3);

            return str.EndsWith("__") && hasSymbolsExceptUnderscore;
        }

        public static bool EndsWithSingleUnderscore(string str, char escapeSymbol)
        {
            if (str == null)
                return false;

            var penultimateSymbolIsNotSpecial =
                str.Length > 1 && str[str.Length - 2] != '_' || str.Length > 2 && str[str.Length - 3] == escapeSymbol;

            var hasSymbolsExceptUnderscore =
                penultimateSymbolIsNotSpecial && (str.Length == 2 && str[0] != escapeSymbol || str.Length > 2);

            return str.EndsWith("_") && hasSymbolsExceptUnderscore;
        }
    }
}
