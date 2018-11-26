using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenParser
    {
        public List<Token> GetTokens(string mdText, List<TokenInformation> data)
        {
            var allTokens = new List<Token>();
            for (var i = 0; i < mdText.Length; i++)
            {
                var symbolFromText = mdText[i];
                if (char.IsLetterOrDigit(symbolFromText) || char.IsWhiteSpace(symbolFromText))
                    continue;
                {
                    var symbolInfo = GetSymbolInformation(i, mdText, data);
                    if (symbolInfo == null)
                        continue;
                    if (symbolInfo.Symbol == "\\")
                    {
                        var nxtSymbol = GetSymbolInformation(i + 1, mdText, data);
                        if (IsInBase(data, nxtSymbol))
                        {
                            allTokens.Add(new Token(symbolInfo, TokenType.Escaped, i));
                            allTokens.Add(new Token(nxtSymbol, TokenType.Ordinary, i+1));
                            i += nxtSymbol.CountOfSpaces;
                        }
                        else
                        {
                            allTokens.Add(new Token(symbolInfo, TokenType.Start, i));
                        }

                        continue;
                    }

                    Token token;
                    if (i == 0)
                    {
                        if (char.IsNumber(mdText[i + symbolInfo.CountOfSpaces]))
                        {
                            i += symbolInfo.CountOfSpaces - 1;
                            continue;
                            
                        }
                        if (!char.IsWhiteSpace(mdText[i + symbolInfo.CountOfSpaces]))
                        {
                            token = new Token(symbolInfo, TokenType.Start, i);
                            allTokens.Add(token);
                            i += token.Data.CountOfSpaces - 1;
                        }

                        i += symbolInfo.CountOfSpaces - 1;
                        continue;
                    }

                    if (i + symbolInfo.CountOfSpaces >= mdText.Length)
                    {
                        if (char.IsNumber(mdText[i - symbolInfo.CountOfSpaces]))
                        {
                            i += symbolInfo.CountOfSpaces - 1;
                            continue;
                        }
                       else if (!char.IsWhiteSpace(mdText[i - 1]))
                        {
                            token = new Token(symbolInfo, TokenType.End, i);
                            allTokens.Add(token);
                            i += token.Data.CountOfSpaces - 1;
                        }
                        
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
                    if (char.IsWhiteSpace(prevSymbol) && !char.IsWhiteSpace(nextSymbol))
                        token = new Token(symbolInfo, TokenType.Start, i);
                    else if (!char.IsWhiteSpace(prevSymbol) && char.IsWhiteSpace(nextSymbol))
                        token = new Token(symbolInfo, TokenType.End, i);
                    else
                    {
                        i+=symbolInfo.CountOfSpaces - 1;
                        continue;
                    }
                    allTokens.Add(token);
                    i += token.Data.CountOfSpaces - 1;
                }
            }

            return allTokens;
        }

        private bool IsInBase(List<TokenInformation> data, TokenInformation symbol)
        {
            foreach (var baseData in data)
            {
                if (baseData.Symbol == symbol.Symbol)
                    return true;
            }

            return false;
        }

        private TokenInformation GetSymbolInformation(int index, string mdText, List<TokenInformation> data)
        {
            var sbl = GetAllCharInSymbol(index, mdText);
            foreach (var baseData in data)
                for (var i = 0; i < sbl.Length; i++)
                {
                    if (baseData.CountOfSpaces != sbl.Length)
                        break;
                    if (mdText[index + i] != baseData.Symbol[i])
                        break;

                    if (i == sbl.Length - 1) return baseData;
                }
            return null;
        }

        private string GetAllCharInSymbol(int index, string mdText)
        {
            var symbol = mdText[index];
            var str = new StringBuilder(symbol.ToString());
            for (var i = index + 1; i < mdText.Length; i++)
            {
                var value = mdText[i];
                if (value != symbol) return str.ToString();

                str.Append(value);
            }

            return str.ToString();
        }
    }
}