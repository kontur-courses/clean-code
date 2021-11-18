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

        internal List<Token> Result;
        internal string TextToParse;
        internal Dictionary<Type, Token> Tokens;

        private void Initialize(string toParse)
        {
            Tokens = new Dictionary<Type, Token>();
            Result = new List<Token>();
            TextToParse = toParse;
        }

        public IEnumerable<Token> ParseTokens(string input)
        {
            Initialize(input);
            var possibleTag = new StringBuilder();

            for (var i = 0; i < TextToParse.Length; i++)
            {
                var symbol = TextToParse[i];

                if (symbol == '\n')
                {
                    Tokens.Clear();
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
                    Tokens.Remove(typeof(ScreeningToken));

                if (i == TextToParse.Length - 1 && possibleTag.Length > 0)
                {
                    var type = TokenTypes[possibleTag.ToString()];
                    ProcessToken(type, i - possibleTag.Length + 1, possibleTag.Length);
                }
            }

            return Result;
        }

        private void ProcessToken(Type type, int index, int length)
        {
            if (ExecuteScreening()) return;

            if (type.IsSubclassOf(typeof(NonPairedToken)))
            {
                ((Token)Activator.CreateInstance(type, index))?.Accept(this);
                return;
            }

            if (Tokens.TryGetValue(type, out var token))
            {
                if (!IsCorrectClosePosition(index, TextToParse)) return;

                token.Close(index);
                token.Accept(this);
            }
            else
            {
                if (!IsCorrectOpenPosition(index, TextToParse, length)) return;

                Tokens[type] = (Token)Activator.CreateInstance(type, index);
            }
        }

        private static bool IsCorrectOpenPosition(int openIndex, string text, int length)
        {
            var indexNextToSeparator = openIndex + length;

            return openIndex != text.Length - 1 && indexNextToSeparator < text.Length && text[indexNextToSeparator] != ' ';
        }

        private static bool IsCorrectClosePosition(int closeIndex, string text)
        {
            return closeIndex != 0 && text[closeIndex - 1] != ' ';
        }

        internal void Visit(HeaderToken token)
        {
            if (token.OpenIndex != 0 && TextToParse[token.OpenIndex - 1] != '\n')
                return;

            var closeIndex = TextToParse.IndexOf('\n');

            if (closeIndex == -1)
                closeIndex = TextToParse.Length;

            token.Close(closeIndex);
            Result.Add(token);
        }

        internal void Visit(ItalicToken token)
        {
            var isTokenCorrect = token.IsTokenPlacedCorrectly(TextToParse);

            if (isTokenCorrect && Tokens.TryGetValue(typeof(BoldToken), out var boldToken))
            {
                if (token.OpenIndex < boldToken.OpenIndex && boldToken.OpenIndex < token.CloseIndex)
                    isTokenCorrect = false;
            }

            Tokens.Remove(token.GetType());

            if (isTokenCorrect)
                Result.Add(token);
        }

        internal void Visit(BoldToken token)
        {
            var isTokenCorrect = token.IsTokenPlacedCorrectly(TextToParse);

            if (isTokenCorrect && Tokens.TryGetValue(typeof(ItalicToken), out var italicToken))
            {
                if (token.OpenIndex < italicToken.OpenIndex && italicToken.OpenIndex < token.CloseIndex)
                    isTokenCorrect = false;

                if (italicToken.OpenIndex < token.OpenIndex && italicToken.IsOpened)
                    isTokenCorrect = false;
            }

            Tokens.Remove(token.GetType());

            if (isTokenCorrect)
                Result.Add(token);
        }

        internal void Visit(ScreeningToken token)
        {
            Tokens.Add(token.GetType(), token);
        }

        private bool ExecuteScreening()
        {
            if (!Tokens.ContainsKey(typeof(ScreeningToken))) return false;

            Tokens.Remove(typeof(ScreeningToken), out var token);
            token.Close(token.OpenIndex);
            Result.Add(token);
            return true;
        }
    }
}