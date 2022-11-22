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
        Backslash,
        Space,
        Newline,
        Underscore,
        Sharp,
        Highlight
    }

    internal enum HighlightTokenType
    {
        Italic,
        Strong
    }

    internal class Token
    {
        public virtual TokenType Type { get; set; }
        public string Text { get; set; }
    }

    internal class HighlightToken : Token
    {
        public override TokenType Type => TokenType.Highlight;
        public HighlightTokenType HighlightType { get; set; }

        public bool IsClosing { get; set; }
    }

    internal class MarkdownToTokenParser
    {
        private const char shieldSymbol = '\\';

        private readonly Dictionary<HighlightTokenType, bool> highlightOpen = new Dictionary<HighlightTokenType, bool>()
        {
            { HighlightTokenType.Italic, false },
            { HighlightTokenType.Strong, false }
        };

        private static readonly Dictionary<char, TokenType> symbolToTokenTypes = new Dictionary<char, TokenType>()
        {
            { ' ', TokenType.Space },
            { '\n', TokenType.Newline },
            { '_', TokenType.Underscore },
            { '#', TokenType.Sharp }
        };

        private static readonly Dictionary<HighlightTokenType, string> highlightTokenText = new Dictionary<HighlightTokenType, string>()
        {
            { HighlightTokenType.Italic, "_" },
            { HighlightTokenType.Strong, "__" }
        };

        private static readonly Dictionary<HighlightTokenType, HighlightTokenType> highlightCanBeInsideOther = new Dictionary<HighlightTokenType, HighlightTokenType>()
        {
            { HighlightTokenType.Strong, HighlightTokenType.Italic }
        };

        public List<Token> ParseToTokens(string input)
        {
            var tokens = CreateTokens(input);
            tokens = ValidateHighlighting(tokens);
            tokens = ResolveTokenIntersections(tokens);
            return tokens;
        }

        private List<Token> CreateTokens(string input)
        {
            var tokens = new List<Token>(input.Length);
            var reservedSymbols = symbolToTokenTypes.Keys;
            var buffer = new StringBuilder();

            int i;
            for (i = 0; i < input.Length - 1; i++)
            {
                var currentSymbol = input[i];

                var shouldAddSymbolToTextToken = !reservedSymbols.Contains(currentSymbol);
                if (currentSymbol == shieldSymbol && reservedSymbols.Contains(input[i + 1]))
                {
                    currentSymbol = input[i + 1];
                    i++;
                }

                if (shouldAddSymbolToTextToken)
                {
                    buffer.Append(currentSymbol);
                    continue;
                }

                if (buffer.Length > 0)
                {
                    tokens.Add(CreateTextToken(buffer.ToString()));
                    buffer.Clear();
                }

                var token = CreateServiceToken(currentSymbol, input[i + 1]);
                tokens.Add(token);
                i += token.Text.Length - 1;
            }

            if (!reservedSymbols.Contains(input.Last()))
            {
                buffer.Append(input.Last());
                i++;
            }

            if (buffer.Length > 0)
                tokens.Add(CreateTextToken(buffer.ToString()));

            if (i != input.Length)
                tokens.Add(CreateServiceToken(input.Last(), '\0'));

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
            if (symbols[0] == '_')
            {
                var currentTokenType = symbols[1] == '_' ? HighlightTokenType.Strong : HighlightTokenType.Italic;

                var currentToken = new HighlightToken()
                {
                    HighlightType = currentTokenType,
                    IsClosing = highlightOpen[currentTokenType],
                    Text = highlightTokenText[currentTokenType]
                };
                highlightOpen[currentTokenType] = !highlightOpen[currentTokenType];

                return currentToken;
            }

            return new Token() { Type = symbolToTokenTypes[symbols[0]], Text = symbols[0].ToString() };
        }

        private static List<Token> ValidateHighlighting(List<Token> tokens)
        {
            for (int openingTokenIndex = 0; openingTokenIndex < tokens.Count; openingTokenIndex++)
            {
                var currentToken = tokens[openingTokenIndex] as HighlightToken;
                if (currentToken == null)
                    continue;

                if (currentToken.IsClosing)
                    continue;

                var closingTokenIndex = tokens.FindIndex(openingTokenIndex,
                                                         token => token is HighlightToken &&
                                                                  (token as HighlightToken)!.HighlightType == currentToken.HighlightType &&
                                                                  (token as HighlightToken)!.IsClosing)!;
                var closingToken = tokens[closingTokenIndex];
                if (closingToken == null)
                {
                    tokens[openingTokenIndex] = new Token()
                    {
                        Type = TokenType.Text,
                        Text = highlightTokenText[currentToken.HighlightType]
                    };

                    continue;
                }

                // ____
                if (currentToken.HighlightType == HighlightTokenType.Strong)
                {
                    if (closingTokenIndex - openingTokenIndex > 1)
                        continue;

                    tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = highlightTokenText[currentToken.HighlightType] };
                    tokens[openingTokenIndex + 1] = new Token() { Type = TokenType.Text, Text = tokens[openingTokenIndex].Text };

                    continue;
                }

                if (currentToken.HighlightType != HighlightTokenType.Italic)
                    continue;

                // _text _
                // _ text_
                if (tokens[openingTokenIndex + 1].Type == TokenType.Space ||
                   tokens[closingTokenIndex - 1].Type == TokenType.Space)
                {
                    tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = highlightTokenText[currentToken.HighlightType] };
                    tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = tokens[openingTokenIndex].Text };
                    continue;
                }

                var hasTextOnTheLeft = openingTokenIndex > 0 && tokens[openingTokenIndex - 1].Type == TokenType.Text;
                var hasTextOnTheRight = closingTokenIndex < tokens.Count - 1 &&
                                        tokens[closingTokenIndex + 1].Type == TokenType.Text;
                var hasSpacesBetween = tokens.FindIndex(openingTokenIndex,
                                                        closingTokenIndex - openingTokenIndex + 1,
                                                        token => token.Type == TokenType.Space) > 0;

                // a_a a_a
                // _a a_a | a _a a_a
                // a_a a_ | a_a a_ a
                if (hasTextOnTheLeft && hasTextOnTheRight && hasSpacesBetween ||
                    !hasTextOnTheLeft && hasTextOnTheRight ||
                    hasTextOnTheLeft && !hasTextOnTheRight)
                {
                    tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = highlightTokenText[currentToken.HighlightType] };
                    tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = tokens[openingTokenIndex].Text };
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

                tokens[openingTokenIndex] = new Token() { Type = TokenType.Text, Text = highlightTokenText[currentToken.HighlightType] };
                tokens[closingTokenIndex] = new Token() { Type = TokenType.Text, Text = tokens[openingTokenIndex].Text };
            }

            return tokens;
        }

        private static List<Token> ResolveTokenIntersections(List<Token> tokens)
        {
            var highlightingTokens = tokens.Where(token => token is HighlightToken)
                                           .Select(token => (token as HighlightToken)!)
                                           .ToArray();
            var highlightingTokenPositions = highlightingTokens.Select(token => tokens.IndexOf(token)).ToArray();

            var stack = new List<HighlightTokenType>();

            int stackOpeningTokenIndex;
            int stackClosingTokenIndex;

            for (int currentTokenIndex = 0; currentTokenIndex < highlightingTokens.Length; currentTokenIndex++)
            {
                var currentToken = highlightingTokens[currentTokenIndex];
                if (!currentToken.IsClosing &&
                   (stack.Count < 1 ||
                      highlightCanBeInsideOther.ContainsKey(stack.Last()) &&
                      highlightCanBeInsideOther[stack.Last()] == currentToken.HighlightType))
                {
                    stack.Add(currentToken.HighlightType);
                    continue;
                }

                if (!currentToken.IsClosing)
                {
                    var currentClosingTokenIndex = Array.FindIndex(highlightingTokens,
                                                                   currentTokenIndex,
                                                                   token => token.HighlightType == currentToken.HighlightType &&
                                                                            token.IsClosing);

                    stackClosingTokenIndex = Array.FindIndex(highlightingTokens,
                                                             currentTokenIndex,
                                                             token => token.HighlightType == stack.Last() &&
                                                                      token.IsClosing);

                    tokens[highlightingTokenPositions[currentTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    tokens[highlightingTokenPositions[currentClosingTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };

                    if (stackClosingTokenIndex < currentClosingTokenIndex)
                    {
                        stackOpeningTokenIndex = Array.FindLastIndex(highlightingTokens, 0,
                                                                        currentTokenIndex,
                                                                        token => token.HighlightType == stack.Last() &&
                                                                                 !token.IsClosing);
                        tokens[highlightingTokenPositions[stackOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                        tokens[highlightingTokenPositions[stackClosingTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    }

                    continue;
                }

                if (currentToken.HighlightType == stack.Last())
                {
                    stack.Remove(stack.Last());
                    continue;
                }

                stackOpeningTokenIndex = Array.FindLastIndex(highlightingTokens,
                                                             currentTokenIndex,
                                                             token => token.HighlightType == stack.Last() &&
                                                                      !token.IsClosing);
                stackClosingTokenIndex = Array.FindIndex(highlightingTokens,
                                                            currentTokenIndex,
                                                            token => token.HighlightType == stack.Last() &&
                                                                     token.IsClosing);

                var currentOpeningTokenIndex = Array.FindLastIndex(highlightingTokens,
                                                                   currentTokenIndex,
                                                                   token => token.HighlightType == currentToken.HighlightType &&
                                                                            !token.IsClosing);

                if (currentOpeningTokenIndex < stackOpeningTokenIndex || currentTokenIndex > stackClosingTokenIndex)
                {
                    tokens[highlightingTokenPositions[stackOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                    tokens[highlightingTokenPositions[stackClosingTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                }

                tokens[highlightingTokenPositions[currentOpeningTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };
                tokens[highlightingTokenPositions[currentTokenIndex]] = new Token() { Type = TokenType.Text, Text = string.Empty };

                stack.Remove(currentToken.HighlightType);
            }

            return tokens;
        }
    }

    internal static class TokenToHtmlParser
    {
        private const string openTagFormat = "<{0}>";
        private const string closeTagFormat = "</{0}>";

        private static readonly Dictionary<HighlightTokenType, string> highlightingTagBodies = new Dictionary<HighlightTokenType, string>()
        {
            { HighlightTokenType.Italic, "em" },
            { HighlightTokenType.Strong, "strong" }
        };

        public static string GetHtmlTextFromTokens(List<Token> tokens)
        {
            var htmlText = new StringBuilder();

            foreach (var token in tokens)
            {
                if (!(token is HighlightToken))
                {
                    htmlText.Append(token.Text);
                    continue;
                }

                var highlightToken = (token as HighlightToken)!;
                htmlText.AppendFormat(highlightToken.IsClosing ? closeTagFormat : openTagFormat,
                                      highlightingTagBodies[highlightToken.HighlightType]);
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