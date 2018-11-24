using System;
using System.Collections.Generic;


namespace Markdown
{
    class Parser
    {
        private List<StringPart> _result;
        private StringPart _lastStringPart;
        public readonly char EscapeSymbol;

        public Parser(char escapeSymbol = '/')
        {
            EscapeSymbol = escapeSymbol;
            _result = new List<StringPart>();
            _lastStringPart = null;
        }

        public List<StringPart> Parse(string str)
        {
            if (str.Length == 0)
                return new List<StringPart>() {new StringPart("", ActionType.NotAnAction, TagType.String)};

            var result = new List<StringPart>();
            for (var i = 0; i < str.Length; i++)
            {
                var subStr = GetSubString(str, i);
                i += subStr.Length - 1;

                result.AddRange(ParseSubString(subStr));
                _result = new List<StringPart>();
                _lastStringPart = null;
            }

            return result;
        }

        public List<StringPart> ParseSubString(string subString)
        {
            if (subString.Length == 0 || IsWhitespace(subString[0]) || StrContainsSymbolsAndDigits(subString))
                return new List<StringPart>() { new StringPart(subString, ActionType.NotAnAction, TagType.String) };

            subString = AddToResultFirstAndLastItems(subString);

            var tmp = ParseSubStringBody(subString, EscapeSymbol);
            _result.AddRange(tmp);

            if (_lastStringPart != null)
                _result.Add(_lastStringPart);

            return _result;
        }

        public static List<StringPart> ParseSubStringBody(string subString, char escapeSymbol)
        {
            var result = new List<StringPart>();
            var length = subString.Length;
            for (var i = 0; i < length; i++)
            {
                var nextSpecialIndex = FindNextSpecialSymbolIndex(subString, i, '_');

                if (i != nextSpecialIndex)
                    result.Add(new StringPart(
                        subString.Substring(i, nextSpecialIndex - i).ClearFromSymbol(escapeSymbol),
                        ActionType.NotAnAction, TagType.String));

                if (nextSpecialIndex == length)
                    break;

                //тут встретили специальный символ
                i = nextSpecialIndex;

                if (i + 1 < length && subString[i + 1] == '_') // case _ _ 
                {
                    if (i + 2 < length)
                    {
                        if (subString[i + 2] == '_')
                            result.Add(new StringPart("_", ActionType.NotAnAction, TagType.String));
                        else
                        {
                            result.Add(new StringPart("__", ActionType.OpenOrClose, TagType.Strong));
                            i++;
                        }
                    }
                    else
                    {
                        result.Add(new StringPart("__", ActionType.NotAnAction, TagType.String));
                        break;
                    }
                }
                else
                    result.Add(i + 1 < length
                        ? new StringPart("_", ActionType.OpenOrClose, TagType.Em)
                        : new StringPart("_", ActionType.NotAnAction, TagType.String));
            }

            return result;
        }

        public static int FindNextSpecialSymbolIndex
            (string subString, int currentIndex, char specialSymbol, char escapeSymbol = '/')
        {
            for (; currentIndex < subString.Length; currentIndex++)
            {
                var isPreviousSymbolIsNotEscapeSymbol = currentIndex - 1 < 0 || subString[currentIndex - 1] != escapeSymbol;
                if (subString[currentIndex] == specialSymbol && isPreviousSymbolIsNotEscapeSymbol)
                    return currentIndex;
            }

            return subString.Length;
        }
        
        public static string GetSubString(string str, int startIndex)
        {
            if (str == null)
                throw new ArgumentNullException();

            if (str.Length == 0)
                return str;

            var onlyWhitespaces = IsWhitespace(str[startIndex]);

            var result = "";
            for (; startIndex < str.Length; startIndex++)
            {
                if (IsWhitespace(str[startIndex]) == onlyWhitespaces)
                    result += str[startIndex];
                else
                    break;
            }

            return result;
        }

        public string AddToResultFirstAndLastItems(string subString)
        {
            if (IsStartsWithDoubleUnderscore(subString))
            {
                _result.Add(new StringPart("__", ActionType.Open, TagType.Strong));
                subString = subString.Substring(2);
            }
            else if (IsStartsWithSingleUnderscore(subString))
            {
                _result.Add(new StringPart("_", ActionType.Open, TagType.Em));
                subString = subString.Substring(1);
            }

            if (IsEndsWithDoubleUnderscore(subString))
            {
                _lastStringPart = new StringPart("__", ActionType.Close, TagType.Strong);
                subString = subString.Substring(0, subString.Length - 2);
            }
            else if (IsEndsWithSingleUnderscore(subString))
            {
                _lastStringPart = new StringPart("_", ActionType.Close, TagType.Em);
                subString = subString.Substring(0, subString.Length - 1);
            }

            return subString;
        }

        public static bool IsWhitespace(char symbol)
        {
            return symbol == ' ' || symbol == '\n';
        }

        public static bool StrContainsSymbolsAndDigits(string str)
        {
            if (str == null)
                return false;

            var containSymbols = false;
            var containsDigits = false;

            foreach (var symbol in str)
                if (!IsWhitespace(symbol))
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
            return symbol == '0' ||
                   symbol == '1' ||
                   symbol == '2' ||
                   symbol == '3' ||
                   symbol == '4' ||
                   symbol == '5' ||
                   symbol == '6' ||
                   symbol == '7' ||
                   symbol == '8' ||
                   symbol == '9';
        }

        public  bool IsStartsWithDoubleUnderscore(string str)
        {
            if (str == null)
                return false;

            var hasSymbolsExceptUnderscore =
                str.Length > 2 && str[2] != '_' && (str.Length == 3 && str[2] != EscapeSymbol || str.Length > 3);

            return str.StartsWith("__") && hasSymbolsExceptUnderscore;
        }

        public bool IsStartsWithSingleUnderscore(string str)
        {
            if (str == null)
                return false;

            var hasSymbolsExceptUnderscore =
                str.Length > 1 && str[1] != '_' && (str.Length == 2 && str[1] != EscapeSymbol || str.Length > 2);

            return str.StartsWith("_") && hasSymbolsExceptUnderscore;
        }

        public bool IsEndsWithDoubleUnderscore(string str)
        {
            if (str == null)
                return false;

            var prePenultimateSymbolIsNotSpecial =
                str.Length > 2 && str[str.Length - 3] != '_' || str.Length > 3 && str[str.Length - 4] == EscapeSymbol;

            var hasSymbolsExceptUnderscore =
                prePenultimateSymbolIsNotSpecial && (str.Length == 3 && str[0] != EscapeSymbol || str.Length > 3);

            return str.EndsWith("__") && hasSymbolsExceptUnderscore;
        }

        public bool IsEndsWithSingleUnderscore(string str)
        {
            if (str == null)
                return false;

            var penultimateSymbolIsNotSpecial =
                str.Length > 1 && str[str.Length - 2] != '_' || str.Length > 2 && str[str.Length - 3] == EscapeSymbol;

            var hasSymbolsExceptUnderscore =
                penultimateSymbolIsNotSpecial && (str.Length == 2 && str[0] != EscapeSymbol || str.Length > 2);

            return str.EndsWith("_") && hasSymbolsExceptUnderscore;
        }
    }
}
