using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenParser
    {
        public List<Token> GetTokens(string mdText, List<TokenInformation> baseTokens)
        {
            if (baseTokens.Count == 0)
                return new List<Token>();
            var allTokens = new List<Token>();
            for (var i = 0; i < mdText.Length; i++)
            {
                var symbolFromText = mdText[i];
                if (char.IsLetterOrDigit(symbolFromText) || char.IsWhiteSpace(symbolFromText))
                    continue;

                var symbolInfo = GetSymbolInformation(i, mdText, baseTokens);
                if (symbolInfo == null)
                    continue;

                var doubleUnderscore = "__";
                var underscore = "_";
                if (symbolInfo.Symbol == doubleUnderscore)
                {
                    var countOfSymbols = GetAllCharInSymbol(i, mdText, baseTokens, true);
                    if (countOfSymbols == 3)
                    {
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

                        i += countOfSymbols - 1;
                        continue;
                    }
                }

                if (symbolInfo.EndIsNewLine)
                {
                    var endPos = GetEndPosition(i, mdText);
                    allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                    allTokens.Add(new Token(symbolInfo, TokenType.End, endPos));

                    continue;
                }

                if (symbolInfo.Symbol == "\\")
                {
                    var nextSymbolInfo = GetSymbolInformation(i + 1, mdText, baseTokens);
                    if (nextSymbolInfo == null)
                        continue;
                    if (baseTokens.Any(s => s.Symbol == nextSymbolInfo.Symbol))
                    {
                        allTokens.Add(new Token(symbolInfo, TokenType.Escaped, i));
                        allTokens.Add(new Token(nextSymbolInfo, TokenType.Ordinary, i + 1));
                        i += nextSymbolInfo.CountOfSpaces;
                    }
                    else
                    {
                        allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                    }

                    continue;
                }

                if (i == 0)
                {
                    if (!char.IsNumber(mdText[i + symbolInfo.CountOfSpaces]))
                        if (!char.IsWhiteSpace(mdText[i + symbolInfo.CountOfSpaces]))
                            allTokens.Add(new Token(symbolInfo, TokenType.Start, i));

                    i += symbolInfo.CountOfSpaces - 1;
                    continue;
                }

                if (i + symbolInfo.CountOfSpaces >= mdText.Length)
                {
                    if (!char.IsNumber(mdText[i - symbolInfo.CountOfSpaces]))
                        if (!char.IsWhiteSpace(mdText[i - 1]))
                            allTokens.Add(new Token(symbolInfo, TokenType.End, i));

                    i += symbolInfo.CountOfSpaces - 1;
                    continue;
                }

                var prevSymbol = mdText[i - 1];
                var nextSymbol = mdText[i + symbolInfo.CountOfSpaces];
                if (char.IsNumber(prevSymbol) || char.IsNumber(nextSymbol))
                {
                    i += symbolInfo.CountOfSpaces - 1;
                    continue;
                }

                if (IsToken(i, prevSymbol, nextSymbol, symbolInfo, out var token))
                    allTokens.Add(token);

                i += symbolInfo.CountOfSpaces - 1;
            }

            return allTokens;
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
            var endPos = mdText.IndexOf("\n", startPos);
            if (endPos == -1)
                return mdText.Length - 1;
            return endPos;
        }

        private TokenInformation GetSymbolInformation(int index, string mdText, List<TokenInformation> baseTokens)
        {
            var symbolLength = GetAllCharInSymbol(index, mdText, baseTokens);
            var symbol = mdText.Substring(index, symbolLength);
            return baseTokens.FirstOrDefault(s => s.CountOfSpaces == symbolLength && s.Symbol == symbol);
        }

        private int GetAllCharInSymbol(int index, string mdText, List<TokenInformation> data, bool toEndOfText = false)
        {
            var maxSymbolsLength = data.Max(s => s.CountOfSpaces);
            var upperBound = toEndOfText ? mdText.Length : Math.Min(index + maxSymbolsLength, mdText.Length);
            var symbol = mdText[index];
            var str = new StringBuilder(symbol.ToString());
            for (var i = index + 1; i < upperBound; i++)
            {
                var value = mdText[i];
                if (value != symbol) return str.Length;

                str.Append(value);
            }

            return str.Length;
        }
    }
}