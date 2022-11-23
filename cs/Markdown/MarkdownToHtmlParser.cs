using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal enum TokenType
    {
        Text,
        Numbers,
        Space,
        Newline,
        Sharp,
        Object
    }

    internal enum TokenObjectType
    {
        Italic,
        Strong,
        Header
    }

    internal class Token
    {
        public virtual TokenType Type { get; set; }
        public string Text { get; set; }
    }

    internal class ObjectOpenToken : Token
    {
        public override TokenType Type => TokenType.Object;
        public TokenObjectType ObjectType { get; set; }
    }

    internal class ObjectCloseToken : Token
    {
        public override TokenType Type => TokenType.Object;
        public TokenObjectType ObjectType { get; set; }
    }

    internal class ObjectOpenCloseStrings
    {
        public string Open { get; set; }
        public string Close { get; set; }
    }

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

        private static readonly Dictionary<TokenObjectType, List<TokenObjectType>> objectsCanBeInsideOtherObject = new Dictionary<TokenObjectType, List<TokenObjectType>>()
      {
         { TokenObjectType.Strong, new List<TokenObjectType>() { TokenObjectType.Italic } },
         { TokenObjectType.Header, new List<TokenObjectType>() { TokenObjectType.Italic, TokenObjectType.Strong } }
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
            var tokens = CreateTokens(input);
            tokens = ValidateServiceTokens(tokens);
            tokens = ResolveTokenIntersections(tokens);
            return tokens;
        }

        private List<Token> CreateTokens(string input)
        {
            var tokens = new List<Token>(input.Length);
            var buffer = new StringBuilder();

            int symbolIndex;
            for (symbolIndex = 0; symbolIndex < input.Length - 1; symbolIndex++)
            {
                var currentSymbol = input[symbolIndex];

                var isCurrentSymbolReserved = reservedSymbols.Contains(currentSymbol);
                var isNextSymbolShielded = currentSymbol == shieldSymbol && shouldBeShieldSymbols.Contains(input[symbolIndex + 1]);
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
                    tokens.Add(CreateTextToken(buffer.ToString()));
                    buffer.Clear();
                }

                var token = CreateServiceToken(currentSymbol, input[symbolIndex + 1]);
                tokens.Add(token);
                symbolIndex += token.Text.Length - 1;
            }

            var lastSymbol = input.Last();
            if (!reservedSymbols.Contains(lastSymbol))
            {
                buffer.Append(lastSymbol);
                symbolIndex++;
            }

            if (buffer.Length > 0)
                tokens.Add(CreateTextToken(buffer.ToString()));

            if (symbolIndex != input.Length)
                tokens.Add(CreateServiceToken(lastSymbol, '\0'));

            return tokens;
        }

        private static Token CreateTextToken(string text)
        {
            return new Token()
            {
                Type = TokenType.Text,
                Text = text
            };
        }

        private Token CreateServiceToken(params char[] symbols)
        {
            var tokenText = new string(symbols);

            foreach (var pair in openCloseObjectStrings)
            {
                var type = pair.Key;
                var openCloseStrings = pair.Value;

                var opened = objectOpen[type];
                if (tokenText == openCloseStrings.Open && !opened)
                {
                    objectOpen[type] = !objectOpen[type];
                    return new ObjectOpenToken() { ObjectType = type, Text = tokenText };
                }

                if (tokenText == openCloseStrings.Close && opened)
                {
                    objectOpen[type] = !objectOpen[type];
                    return new ObjectCloseToken() { ObjectType = type, Text = tokenText };
                }
            }

            tokenText = tokenText.Substring(0, 1);

            if (symbols[0] == '_')
            {
                var tokenType = TokenObjectType.Italic;
                var opened = objectOpen[tokenType];
                var token = opened ? (Token)new ObjectCloseToken() { ObjectType = tokenType, Text = tokenText } :
                                     (Token)new ObjectOpenToken() { ObjectType = tokenType, Text = tokenText };
                objectOpen[tokenType] = !objectOpen[tokenType];
                return token;
            }

            if (symbols[0] == '\n')
            {
                if (!objectOpen[TokenObjectType.Header])
                    return new Token() { Type = reservedSymbolTokenType[symbols[0]], Text = tokenText };

                var tokenType = TokenObjectType.Header;
                objectOpen[tokenType] = !objectOpen[tokenType];
                return new ObjectCloseToken() { ObjectType = tokenType, Text = tokenText };
            }

            return new Token() { Type = reservedSymbolTokenType[symbols[0]], Text = tokenText };
        }

        private static List<Token> ValidateServiceTokens(List<Token> tokens)
        {
            for (int openingTokenIndex = 0; openingTokenIndex < tokens.Count; openingTokenIndex++)
            {
                var currentToken = tokens[openingTokenIndex] as ObjectOpenToken;
                if (currentToken == null)
                    continue;

                var closingTokenIndex = tokens.FindIndex(openingTokenIndex,
                                                         token => token is ObjectCloseToken &&
                                                                  (token as ObjectCloseToken)!.ObjectType == currentToken.ObjectType);
                if (closingTokenIndex < 0)
                {
                    tokens[openingTokenIndex] = new Token()
                    {
                        Type = TokenType.Text,
                        Text = currentToken.Text
                    };

                    continue;
                }

                var closingToken = tokens[closingTokenIndex];
                // ____
                if (currentToken.ObjectType == TokenObjectType.Strong)
                {
                    if (closingTokenIndex - openingTokenIndex == 1)
                    {
                        tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                        tokens[openingTokenIndex + 1] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                    }
                    continue;
                }

                if (currentToken.ObjectType == TokenObjectType.Header)
                {
                    if (openingTokenIndex != 0 && tokens[openingTokenIndex - 1].Type != TokenType.Newline)
                    {
                        tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                        tokens[openingTokenIndex + 1] = new Token() { Type = TokenType.Text, Text = closingToken.Text };
                    }
                    continue;
                }

                if (currentToken.ObjectType != TokenObjectType.Italic)
                    continue;

                // _text _
                // _ text_
                if (tokens[openingTokenIndex + 1].Type == TokenType.Space ||
                   tokens[closingTokenIndex - 1].Type == TokenType.Space)
                {
                    tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                    tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = closingToken.Text };
                    continue;
                }

                var hasTextOnTheLeft = openingTokenIndex > 0 && tokens[openingTokenIndex - 1].Type == TokenType.Text;
                var hasTextOnTheRight = closingTokenIndex < tokens.Count - 1 &&
                                        tokens[closingTokenIndex + 1].Type == TokenType.Text;
                var hasSpacesBetween = tokens.FindIndex(openingTokenIndex,
                                                        closingTokenIndex - openingTokenIndex + 1,
                                                        token => token.Type == TokenType.Space) > 0;

                if (openingTokenIndex == 0 && (hasSpacesBetween && !hasTextOnTheRight || !hasSpacesBetween && hasTextOnTheRight) ||
                   closingTokenIndex == tokens.Count - 1 && (!hasTextOnTheLeft && hasSpacesBetween || hasTextOnTheLeft && !hasSpacesBetween))
                {
                    continue;
                }

                // a_a a_a
                // _a a_a | a _a a_a
                // a_a a_ | a_a a_ a
                if (hasTextOnTheLeft && hasTextOnTheRight && hasSpacesBetween ||
                    !hasTextOnTheLeft && hasTextOnTheRight ||
                    hasTextOnTheLeft && !hasTextOnTheRight)
                {
                    tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                    tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = closingToken.Text };
                    continue;
                }

                // _text text_ text
                // text _text text_ text
                // text text _text text_
                if (!hasTextOnTheLeft && !hasTextOnTheRight)
                    continue;

                if (tokens.FindIndex(openingTokenIndex,
                                     closingTokenIndex - openingTokenIndex + 1,
                                     token => token.Type == TokenType.Text && token.Text.Any(c => !char.IsDigit(c))) >= 0)
                {
                    continue;
                }

                tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = closingToken.Text };
            }

            return tokens;
        }

        private static List<Token> ResolveTokenIntersections(List<Token> tokens)
        {
            var objectTokens = tokens.Where(token => token is ObjectOpenToken || token is ObjectCloseToken)
                                           .ToArray();
            var objectTokenPositions = objectTokens.Select(token => tokens.IndexOf(token)).ToArray();

            var stack = new List<(TokenObjectType ObjectType, int OpeningTokenIndex)>();

            int stackOpeningTokenIndex;
            int stackClosingTokenIndex;
            TokenObjectType currentStackObjectType;

            for (int currentTokenIndex = 0; currentTokenIndex < objectTokens.Length; currentTokenIndex++)
            {
                var currentToken = objectTokens[currentTokenIndex] as ObjectOpenToken;
                if (currentToken != null)
                {
                    var canObjectBeOpenedInsideLast = stack.Count < 1 || objectsCanBeInsideOtherObject.ContainsKey(stack.Last().ObjectType) &&
                                                                         objectsCanBeInsideOtherObject[stack.Last().ObjectType].Contains(currentToken.ObjectType);

                    if (canObjectBeOpenedInsideLast)
                    {
                        stack.Add((currentToken.ObjectType, currentTokenIndex));
                        continue;
                    }

                    (currentStackObjectType, stackOpeningTokenIndex) = stack.Last();

                    var closingCurrentTokenIndex = Array.FindIndex(objectTokens,
                                                                   currentTokenIndex,
                                                                   token => token is ObjectCloseToken &&
                                                                            (token as ObjectCloseToken)!.ObjectType == currentToken.ObjectType);

                    tokens[objectTokenPositions[currentTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    tokens[objectTokenPositions[closingCurrentTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };

                    stackClosingTokenIndex = Array.FindIndex(objectTokens,
                                                             currentTokenIndex,
                                                             token => token is ObjectCloseToken &&
                                                                      (token as ObjectCloseToken)!.ObjectType == currentStackObjectType);

                    if (stackClosingTokenIndex < closingCurrentTokenIndex)
                    {
                        tokens[objectTokenPositions[stackOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                        tokens[objectTokenPositions[stackClosingTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    }

                    continue;
                }

                (currentStackObjectType, stackOpeningTokenIndex) = stack.Last();

                var currentCloseToken = (objectTokens[currentTokenIndex] as ObjectCloseToken)!;
                if (currentCloseToken.ObjectType == currentStackObjectType)
                {
                    stack.Remove((currentStackObjectType, stackOpeningTokenIndex));
                    continue;
                }

                stackClosingTokenIndex = Array.FindIndex(objectTokens,
                                                         currentTokenIndex,
                                                         token => token is ObjectCloseToken &&
                                                                  (token as ObjectCloseToken)!.ObjectType == currentStackObjectType);

                var currentOpeningTokenIndex = Array.FindLastIndex(objectTokens,
                                                                   currentTokenIndex,
                                                                   token => token is ObjectOpenToken &&
                                                                            (token as ObjectOpenToken)!.ObjectType ==
                                                                                  currentCloseToken.ObjectType);

                if (currentOpeningTokenIndex < stackOpeningTokenIndex || currentTokenIndex > stackClosingTokenIndex)
                {
                    tokens[objectTokenPositions[stackOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    tokens[objectTokenPositions[stackClosingTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                }

                tokens[objectTokenPositions[currentOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                tokens[objectTokenPositions[currentTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };

                stack.Remove((currentCloseToken.ObjectType, currentOpeningTokenIndex));
            }

            return tokens;
        }
    }

    internal static class TokenToHtmlParser
    {
        private const string openTagFormat = "<{0}>";
        private const string closeTagFormat = "</{0}>";

        private static readonly Dictionary<TokenObjectType, string> objectTagBodies = new Dictionary<TokenObjectType, string>()
      {
          { TokenObjectType.Italic, "em" },
          { TokenObjectType.Strong, "strong" },
          { TokenObjectType.Header, "h1" },
      };

        public static string GetHtmlTextFromTokens(List<Token> tokens)
        {
            var htmlText = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token is ObjectOpenToken)
                {
                    htmlText.AppendFormat(openTagFormat, objectTagBodies[(token as ObjectOpenToken)!.ObjectType]);
                    continue;
                }

                if (token is ObjectCloseToken)
                {
                    htmlText.AppendFormat(closeTagFormat, objectTagBodies[(token as ObjectCloseToken)!.ObjectType]);
                    continue;
                }

                htmlText.Append(token.Text);
            }

            return htmlText.ToString();
        }
    }

    internal class MarkdownToHtmlParser
    {
        public string Render(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"nameof{input} can't be null or empty");

            var markdownParser = new MarkdownToTokenParser();
            var tokens = markdownParser.ParseToTokens(input);
            return TokenToHtmlParser.GetHtmlTextFromTokens(tokens);
        }
    }
}