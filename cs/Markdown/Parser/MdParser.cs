using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class MdParser
    {
        public static readonly Dictionary<string, Type> TokenTypes = new()
        {
            { Token.Separators[typeof(ItalicToken)], typeof(ItalicToken) },
            { Token.Separators[typeof(BoldToken)], typeof(BoldToken) },
            { Token.Separators[typeof(HeaderToken)], typeof(HeaderToken) },
            { Token.Separators[typeof(ScreeningToken)], typeof(ScreeningToken) }
        };

        private List<Token> result;
        private string textToParse;
        private Dictionary<Type, Token> tokens;

        private void Initialize(string toParse)
        {
            tokens = new Dictionary<Type, Token>();
            result = new List<Token>();
            textToParse = toParse;
        }

        public IEnumerable<Token> ParseTokens(string input)
        {
            Initialize(input);
            var possibleTag = new StringBuilder();

            for (var i = 0; i < textToParse.Length; i++)
            {
                var symbol = textToParse[i];

                if (symbol == '\n')
                {
                    tokens.Clear();
                    possibleTag.Clear();
                    continue;
                }

                if (TokenTypes.ContainsKey($"{possibleTag}{symbol}"))
                    possibleTag.Append(symbol);
                else if (TokenTypes.ContainsKey(possibleTag.ToString()))
                {
                    var type = TokenTypes[possibleTag.ToString()];
                    ProcessToken(type, i - possibleTag.Length, possibleTag.Length);
                    possibleTag.Clear();

                    if (TokenTypes.ContainsKey($"{symbol}"))
                        possibleTag.Append(symbol);
                }
                else
                    tokens.Remove(typeof(ScreeningToken));
            }

            if (possibleTag.Length > 0)
            {
                var type = TokenTypes[possibleTag.ToString()];
                ProcessToken(type, textToParse.Length - possibleTag.Length, possibleTag.Length);
            }

            return result;
        }

        private void ProcessToken(Type type, int openIndex, int length)
        {
            if (ExecuteScreening()) return;

            if (type.IsSubclassOf(typeof(NonPairedToken)))
                ((Token)Activator.CreateInstance(type, openIndex))?.Accept(this);
            else if (tokens.TryGetValue(type, out var token))
            {
                if (IsCorrectClosePosition(openIndex, textToParse))
                {
                    token.Close(openIndex);
                    token.Accept(this);
                }
            }
            else
            {
                if (IsCorrectOpenPosition(openIndex, textToParse, length))
                    tokens[type] = (Token)Activator.CreateInstance(type, openIndex);
            }
        }

        internal void Visit(HeaderToken token)
        {
            if (token.OpenIndex != 0 && textToParse[token.OpenIndex - 1] != '\n')
                return;

            var closeIndex = textToParse.IndexOf('\n');

            if (closeIndex == -1)
                closeIndex = textToParse.Length;

            token.Close(closeIndex);
            result.Add(token);
        }

        internal void Visit(BoldToken token)
        {
            VisitStyleToken(token, ValidateBoldTokenInteractions);
        }

        internal void Visit(ItalicToken token)
        {
            VisitStyleToken(token, ValidateStyleTokenInteractions);
        }

        internal void Visit(ScreeningToken token)
        {
            tokens.Add(token.GetType(), token);
        }

        internal void VisitStyleToken(StyleToken token, Action<StyleToken> interactionsValidator)
        {
            token.ValidatePlacedCorrectly(textToParse);

            interactionsValidator.Invoke(token);

            tokens.Remove(token.GetType());

            if (token.IsCorrect)
                result.Add(token);
        }

        private bool ExecuteScreening()
        {
            if (!tokens.ContainsKey(typeof(ScreeningToken))) return false;

            tokens.Remove(typeof(ScreeningToken), out var token);
            token.Close(token.OpenIndex);
            result.Add(token);
            return true;
        }

        private void ValidateStyleTokenInteractions(StyleToken token)
        {
            foreach (var otherToken in tokens.Values)
            {
                if (otherToken is not StyleToken styleToken || otherToken == token)
                    continue;

                if (!token.IsIntersectWith(styleToken)) continue;

                styleToken.IsCorrect = false;
                token.IsCorrect = false;
            }
        }

        private void ValidateBoldTokenInteractions(StyleToken token)
        {
            ValidateStyleTokenInteractions(token);

            if (!token.IsCorrect || !tokens.ContainsKey(typeof(ItalicToken))) return;

            var italicToken = tokens[typeof(ItalicToken)] as ItalicToken;

            if (italicToken.OpenIndex < token.OpenIndex && italicToken.IsOpened)
                token.IsCorrect = false;
        }

        private static bool IsCorrectOpenPosition(int openIndex, string text, int length)
        {
            var indexNextToSeparator = openIndex + length;

            return openIndex != text.Length - 1 && indexNextToSeparator < text.Length &&
                   text[indexNextToSeparator] != ' ';
        }

        private static bool IsCorrectClosePosition(int closeIndex, string text)
        {
            return closeIndex != 0 && text[closeIndex - 1] != ' ';
        }
    }
}