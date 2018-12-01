using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenParser
    {
        private readonly List<TokenInformation> baseTokens;

        public TokenParser(List<TokenInformation> baseTokens)
        {
            this.baseTokens = baseTokens;
        }

        public List<Token> GetTokens(string mdText)
        {
            if (baseTokens.Count == 0)
                return new List<Token>();
            var allTokens = new List<Token>();
            for (var i = 0; i < mdText.Length; i++)
            {
                if (IsOrdinarySymbol(mdText, i, out var symbolInfo))
                    continue;

                if (IsTripleUnderscore(i, mdText))
                {
                    AddTwoTokens(mdText, i, allTokens, symbolInfo);
                    i += 2;
                    continue;
                }

                if (symbolInfo.EndIsNewLine)
                {
                    AddSharpTokens(mdText, i, allTokens, symbolInfo);
                    continue;
                }

                if (symbolInfo.Symbol == "\\")
                {
                    AddEscapedOrOrdinarySymbol(mdText, ref i, allTokens, symbolInfo);
                    continue;
                }

                if (i == 0)
                {
                    AddStartTokenAtBeginningLine(mdText, i, symbolInfo, allTokens);
                    i += symbolInfo.Symbol.Length - 1;
                    continue;
                }

                if (i + symbolInfo.Symbol.Length >= mdText.Length)
                {
                    AddEndTokenAtLineEnd(mdText, i, symbolInfo, allTokens);
                    i += symbolInfo.Symbol.Length - 1;
                    continue;
                }

                var prevSymbol = mdText[i - 1];
                var nextSymbol = mdText[i + symbolInfo.Symbol.Length];
                if (char.IsNumber(prevSymbol) || char.IsNumber(nextSymbol))
                {
                    i += symbolInfo.Symbol.Length - 1;
                    continue;
                }

                if (IsToken(i, prevSymbol, nextSymbol, symbolInfo, out var token))
                    allTokens.Add(token);

                i += symbolInfo.Symbol.Length - 1;
            }

            return allTokens;
        }

        private bool IsTripleUnderscore(int index, string mdText)
        {
            if (index + 3 >= mdText.Length)
                return false;
            var symbols = mdText.Substring(index, 3);
            if (symbols == "___")
                return true;
            return false;
        }

        private bool IsOrdinarySymbol(string mdText, int i, out TokenInformation symbolInfo)
        {
            symbolInfo = new TokenInformation();
            var symbolFromText = mdText[i];
            if (char.IsLetterOrDigit(symbolFromText) || char.IsWhiteSpace(symbolFromText))
                return true;
            symbolInfo = GetSymbolInformation(i, mdText);
            if (symbolInfo == null)
                return true;
            return false;
        }

        private void AddTwoTokens(string mdText, int i, List<Token> allTokens, TokenInformation symbolInfo)
        {
            var countOfSymbols = 3;
            var underscore = "_";
            var underscoreInfo = baseTokens.Find(s => s.Symbol == underscore);
            var posNextSymbol = i + countOfSymbols;
            if (posNextSymbol < mdText.Length && !char.IsWhiteSpace(mdText[posNextSymbol]))
            {
                allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                allTokens.Add(new Token(underscoreInfo, TokenType.Start, i + 2));
            }

            var indexPrevSymbol = i - 1;
            if (indexPrevSymbol >= 0 && !char.IsWhiteSpace(mdText[indexPrevSymbol]))
            {
                allTokens.Add(new Token(underscoreInfo, TokenType.End, i));
                allTokens.Add(new Token(symbolInfo, TokenType.End, i + 1));
            }
        }

        private static void AddEndTokenAtLineEnd(string mdText, int i, TokenInformation symbolInfo,
            List<Token> allTokens)
        {
            if (!char.IsNumber(mdText[i - symbolInfo.Symbol.Length]))
                if (!char.IsWhiteSpace(mdText[i - 1]))
                    allTokens.Add(new Token(symbolInfo, TokenType.End, i));
        }

        private static void AddStartTokenAtBeginningLine(string mdText, int i, TokenInformation symbolInfo,
            List<Token> allTokens)
        {
            if (!char.IsNumber(mdText[i + symbolInfo.Symbol.Length]))
                if (!char.IsWhiteSpace(mdText[i + symbolInfo.Symbol.Length]))
                    allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
        }

        private void AddEscapedOrOrdinarySymbol(string mdText, ref int i, List<Token> allTokens,
            TokenInformation symbolInfo)
        {
            var nextSymbolInfo = GetSymbolInformation(i + 1, mdText);
            if (nextSymbolInfo == null)
                return;
            if (baseTokens.Any(s => s.Symbol == nextSymbolInfo.Symbol))
            {
                allTokens.Add(new Token(symbolInfo, TokenType.Escaped, i));
                allTokens.Add(new Token(nextSymbolInfo, TokenType.Ordinary, i + 1));
                i += nextSymbolInfo.Symbol.Length;
            }
            else
            {
                allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
            }
        }

        private void AddSharpTokens(string mdText, int i, List<Token> allTokens, TokenInformation symbolInfo)
        {
            if (i == 0 || i > 2 && mdText.Substring(i - 2, 2) == Environment.NewLine)
            {
                var endPos = GetEndPosition(i, mdText);
                allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                allTokens.Add(new Token(symbolInfo, TokenType.End, endPos));
            }
        }

        private bool IsToken(int position, char prevSymbol, char nextSymbol, TokenInformation symbolInfo,
            out Token token)
        {
            if (!char.IsWhiteSpace(nextSymbol))
            {
                token = new Token(symbolInfo, TokenType.Start, position);
                return true;
            }

            if (!char.IsWhiteSpace(prevSymbol))
            {
                token = new Token(symbolInfo, TokenType.End, position);
                return true;
            }

            token = null;
            return false;
        }

        private int GetEndPosition(int startPos, string mdText)
        {
            var endPos = mdText.IndexOf(Environment.NewLine, startPos);
            if (endPos == -1)
                return mdText.Length - 1;
            return endPos;
        }

        private TokenInformation GetSymbolInformation(int index, string mdText)
        {
            var symbol = GetAllCharInSymbol(index, mdText);
            return baseTokens.FirstOrDefault(s => s.Symbol == symbol);
        }

        private string GetAllCharInSymbol(int index, string mdText)
        {
            var maxSymbolsLength = baseTokens.Max(s => s.Symbol.Length);
            var upperBound = Math.Min(index + maxSymbolsLength, mdText.Length);
            var symbol = mdText[index];
            var str = new StringBuilder(symbol.ToString());
            for (var i = index + 1; i < upperBound; i++)
            {
                var value = mdText[i];
                if (value != symbol) return str.ToString();

                str.Append(value);
            }

            return str.ToString();
        }
    }
}