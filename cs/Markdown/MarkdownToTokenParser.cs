using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class MarkdownToTokenParser
    {
        private const char shieldSymbol = '\\';

        private static readonly char[] reservedSymbols = new char[]
        {
            ' ',
            '\n',
            '_',
            '#'
        };

        private static readonly char[] shouldBeShieldSymbols = reservedSymbols.Where(c => c != ' ').Append(shieldSymbol).ToArray();

        private static readonly Dictionary<char, TokenType> reservedSymbolTokenType = new Dictionary<char, TokenType>()
        {
           { reservedSymbols[0], TokenType.Space },
           { reservedSymbols[1], TokenType.Newline },
           { reservedSymbols[3], TokenType.Sharp }
        };

        private readonly Dictionary<TokenObjectType, bool> objectOpen = new Dictionary<TokenObjectType, bool>()
        {
           { TokenObjectType.Italic, false },
           { TokenObjectType.Strong, false },
           { TokenObjectType.Header, false }
        };

        private readonly Dictionary<TokenObjectType, ObjectOpenCloseStrings> openCloseObjectStrings = new Dictionary<TokenObjectType, ObjectOpenCloseStrings>()
        {
           { TokenObjectType.Italic, new ObjectOpenCloseStrings() { Open = "_", Close = "_" } },
           { TokenObjectType.Strong, new ObjectOpenCloseStrings() { Open = "__", Close = "__" } },
           { TokenObjectType.Header, new ObjectOpenCloseStrings() { Open = "# ", Close = "\n" } }
        };

        public List<Token> ParseToTokens(string input)
        {
            var tokens = new List<Token>(input.Length);
            var buffer = new StringBuilder();

            int symbolIndex;
            for (symbolIndex = 0; symbolIndex < input.Length - 1; symbolIndex++)
            {
                var currentSymbol = input[symbolIndex];

                var isCurrentSymbolReserved = reservedSymbols.Contains(currentSymbol);
                var isNextSymbolShielded = currentSymbol == shieldSymbol &&
                                           shouldBeShieldSymbols.Contains(input[symbolIndex + 1]);

                if (!isCurrentSymbolReserved || isNextSymbolShielded)
                {
                    if (isNextSymbolShielded)
                    {
                        currentSymbol = input[symbolIndex + 1];
                        symbolIndex++;
                    }

                    buffer.Append(currentSymbol);
                    continue;
                }

                if (buffer.Length > 0)
                {
                    tokens.Add(Token.CreateTextToken(buffer.ToString()));
                    buffer.Clear();
                }

                var token = ParseServiceToken(currentSymbol, input[symbolIndex + 1]);
                tokens.Add(token);
                symbolIndex += token.Text.Length - 1;
            }

            var lastSymbol = input[symbolIndex];
            if (!reservedSymbols.Contains(lastSymbol))
            {
                buffer.Append(lastSymbol);
                symbolIndex++;
            }

            if (buffer.Length > 0)
                tokens.Add(Token.CreateTextToken(buffer.ToString()));

            if (symbolIndex != input.Length)
                tokens.Add(ParseServiceToken(lastSymbol, '\0'));

            return tokens;
        }

        private Token ParseServiceToken(params char[] symbols)
        {
            var tokenText = new string(symbols);

            foreach (var pair in openCloseObjectStrings)
            {
                var tokenType = pair.Key;
                var openCloseStrings = pair.Value;

                if (tokenText != openCloseStrings.Open && tokenText != openCloseStrings.Close)
                    continue;

                return CreateObjectToken(tokenType, tokenText);
            }

            tokenText = tokenText.Substring(0, 1);

            if (symbols[0] == '_')
            {
                var tokenType = TokenObjectType.Italic;
                return CreateObjectToken(tokenType, tokenText);
            }

            if (symbols[0] == '\n')
            {
                if (!objectOpen[TokenObjectType.Header])
                    return new Token() { Type = reservedSymbolTokenType[symbols[0]], Text = tokenText };

                var tokenType = TokenObjectType.Header;
                return CreateObjectToken(tokenType, tokenText);
            }

            return new Token() { Type = reservedSymbolTokenType[symbols[0]], Text = tokenText };
        }

        private Token CreateObjectToken(TokenObjectType type, string tokenText)
        {
            var isClose = objectOpen[type];
            objectOpen[type] = !objectOpen[type];

            return new ObjectToken()
            {
                ObjectType = type,
                IsClose = isClose,
                Text = tokenText
            };
        }
    }
}