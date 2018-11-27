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
            var allTokens = new List<Token>();
            for (var i = 0; i < mdText.Length; i++)
            {
                var symbolFromText = mdText[i];
                if (char.IsLetterOrDigit(symbolFromText) || char.IsWhiteSpace(symbolFromText))
                    continue;

                var symbolInfo = GetSymbolInformation(i, mdText, baseTokens);
                if (symbolInfo == null)
                    continue;
                if (symbolInfo.Symbol == "__")
                {
                    var countOfSymbols = GetAllCharInSymbol(i, mdText, baseTokens, true).Length;
                    if (countOfSymbols == 3)
                    {
                        var underscoreInfo = baseTokens.Find(s => s.Symbol == "_");
                        var posNextSymbol = i + countOfSymbols;
                        if (posNextSymbol < mdText.Length && !char.IsWhiteSpace(mdText[posNextSymbol]))
                        {
                            allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                            allTokens.Add(new Token(underscoreInfo, TokenType.Start, i + 2));
                        }

                        var indexPrevSymbol = i - 1;
                        if (indexPrevSymbol >= 0 && !char.IsWhiteSpace(mdText[indexPrevSymbol]))
                        {
                            allTokens.Add(new Token(symbolInfo, TokenType.End, i));
                            allTokens.Add(new Token(underscoreInfo, TokenType.End, i + 2));
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
                    var nxtSymbol = GetSymbolInformation(i + 1, mdText, baseTokens);
                    if (nxtSymbol == null)
                        continue;
                    if (IsBaseSymbol(baseTokens, nxtSymbol))
                    {
                        allTokens.Add(new Token(symbolInfo, TokenType.Escaped, i));
                        allTokens.Add(new Token(nxtSymbol, TokenType.Ordinary, i + 1));
                        i += nxtSymbol.CountOfSpaces;
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
            //todo
            token = null;
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

            return false;
        }

        private int GetEndPosition(int startPos, string mdText)
        {
            for (var j = startPos + 1; j < mdText.Length; j++)
                //TODO NewLine
                if (mdText[j] == '\n')
                    return j;

            return mdText.Length - 1;
        }

        private bool IsBaseSymbol(List<TokenInformation> data, TokenInformation symbol)
        {
            foreach (var baseData in data)
                if (baseData.Symbol == symbol.Symbol)
                    return true;

            return false;
        }

        private TokenInformation GetSymbolInformation(int index, string mdText, List<TokenInformation> data)
        {
            var symbol = GetAllCharInSymbol(index, mdText, data, false);
            foreach (var baseData in data)
                for (var i = 0; i < symbol.Length; i++)
                {
                    if (baseData.CountOfSpaces != symbol.Length)
                        break;
                    if (mdText[index + i] != baseData.Symbol[i])
                        break;

                    if (i == symbol.Length - 1) return baseData;
                }

            return null;
        }

        private string GetAllCharInSymbol(int index, string mdText, List<TokenInformation> data, bool toEndOfText)
        {
            var maxSymbolsLength = data.Max(s => s.CountOfSpaces);
            var upperBound = toEndOfText ? mdText.Length : Math.Min(index + maxSymbolsLength, mdText.Length);
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