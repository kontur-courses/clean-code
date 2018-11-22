using System;
using System.Collections.Generic;


namespace Markdown
{
    class Parser
    {
        private List<StringPart> _result;
        private bool _escapeSymbol;
        private StringPart _lastStringPart;
        public readonly char EscapeSymbol;

        public Parser(char escapeSymbol)
        {
            EscapeSymbol = escapeSymbol;
            _result = new List<StringPart>();
            _escapeSymbol = false;
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
                _escapeSymbol = false;
                _lastStringPart = null;
            }

            return result;
        }

        private List<StringPart> ParseSubString(string subString)
        {
            if (subString.Length == 0 || IsWhitespace(subString[0]) || StrContainsSymbolsAndDigits(subString))
                return new List<StringPart>() {new StringPart(subString, ActionType.NotAnAction, TagType.String) };

            subString = AddToResultFirstAndLastItems(subString);

            ParseSubStringBody(subString);

            if (_lastStringPart != null)
                _result.Add(_lastStringPart);

            return _result;
        }


        private List<StringPart> ParseSubStringTest(string subString)
        {
            if (subString.Length == 0 || IsWhitespace(subString[0]) || StrContainsSymbolsAndDigits(subString))
                return new List<StringPart>() { new StringPart(subString, ActionType.NotAnAction, TagType.String) };

            subString = AddToResultFirstAndLastItems(subString);

            ParseSubStringBodyTest(subString);

            if (_lastStringPart != null)
                _result.Add(_lastStringPart);

            return _result;
        }

        private void ParseSubStringBodyTest(string subString)
        {
            var tmpResult = "";
            for (var i = 0; i < subString.Length; i++)
            {
                var nextSpecialSymbolIndex = FindNextSpecialSymbolIndex(subString, i);
                if (nextSpecialSymbolIndex == null)
                {
                    tmpResult += subString.Substring(i);
                    break;
                }

            }
            //for (var i = 0; i < subString.Length; i++)
            //{
            //    if (tmpResult[i] != '_')
            //        tmpResult += tmpResult[i];
            //    else
            //    {
            //        if (i - 1 >= 0 && tmpResult[i - 1] == EscapeSymbol)
            //            tmpResult += tmpResult[i];
            //        else if (i + 2 < subString.Length && subString[i + 1] == '_' && subString[i + 2] == '_')
            //            tmpResult += tmpResult[i];
            //        else if (i + 2 < subString.Length && subString[i + 1] == '_' && subString[i + 2] != '_')
            //        {

            //        }
            //    }
            //}
        }

        public int? FindNextSpecialSymbolIndex(string subString, int startIndex)
        {
            var currentIndex = startIndex;
            for (; currentIndex < subString.Length; currentIndex++)
            {
                if (subString[currentIndex] == EscapeSymbol)
                    return currentIndex;
            }

            return null;
        }

        private void ParseSubStringBody(string subString)
        {
            var tmpResult = "";

            foreach (var symbol in subString)
            {
                if (_escapeSymbol)
                {
                    _escapeSymbol = false;
                    tmpResult = AddSymbolAndRecalculateResults(tmpResult, symbol);
                }
                else
                if (symbol == EscapeSymbol)
                    _escapeSymbol = true;
                else
                    tmpResult = symbol != '_' ?
                        AddSymbolAndRecalculateResults(tmpResult, symbol) :
                        AddSpecialSymbolAndRecalculateResults(tmpResult, symbol);
            }

            if (tmpResult.Length != 0)
                _result.Add(new StringPart(tmpResult, ActionType.NotAnAction, TagType.String));
        }

        private string AddSymbolAndRecalculateResults(string tmpResult, char symbol)
        {
            if (tmpResult == "_")
            {
                _result.Add(new StringPart("_", ActionType.Open, TagType.Em));
                return symbol.ToString();
            }

            if (tmpResult == "__")
            {
                _result.Add(new StringPart("__", ActionType.Open, TagType.Strong));
                return symbol.ToString();
            }

            if (tmpResult.EndsWith("__"))
            {
                _result.Add(new StringPart(tmpResult.Substring(0, tmpResult.Length - 2), ActionType.NotAnAction, TagType.String));
                _result.Add(new StringPart("__", ActionType.OpenOrClose, TagType.Strong));
                return symbol.ToString();
            }

            if (tmpResult.EndsWith("_"))
            {
                _result.Add(new StringPart(tmpResult.Substring(0, tmpResult.Length - 1), ActionType.NotAnAction, TagType.String));
                _result.Add(new StringPart("__", ActionType.OpenOrClose, TagType.Em));
                return symbol.ToString();
            }

            return tmpResult + symbol;
        }

        private string AddSpecialSymbolAndRecalculateResults(string tmpResult, char symbol)
        {
            if (tmpResult == "_")
            {
                return tmpResult + symbol;
            }

            if (tmpResult == "__")
            {
                _result.Add(new StringPart("_", ActionType.NotAnAction, TagType.String));
                return tmpResult + symbol;
            }

            if (tmpResult.EndsWith("__"))
            {
                _result.Add(new StringPart(tmpResult.Substring(0, tmpResult.Length - 2), ActionType.NotAnAction, TagType.String));
                _result.Add(new StringPart("__", ActionType.Close, TagType.Strong));
                return symbol.ToString();
            }

            return tmpResult + symbol;
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
