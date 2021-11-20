using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class MdParser
    {
        public static readonly Dictionary<string, Func<int, Token>> TokensBySeparator = new()
        {
            { ItalicToken.Separator, index => new ItalicToken(index) },
            { BoldToken.Separator, index => new BoldToken(index) },
            { HeaderToken.Separator, index => new HeaderToken(index) },
            { ScreeningToken.Separator, index => new ScreeningToken(index) }
        };

        private List<Token> result;
        private string textToParse;
        private Dictionary<string, Token> tokens;

        public IEnumerable<Token> ParseTokens(string toParse)
        {
            Initialize(toParse);
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

                if (TokensBySeparator.ContainsKey($"{possibleTag}{symbol}"))
                    possibleTag.Append(symbol);
                else if (TokensBySeparator.ContainsKey(possibleTag.ToString()))
                {
                    ProcessToken(possibleTag.ToString(), i - possibleTag.Length);
                    possibleTag.Clear();

                    if (TokensBySeparator.ContainsKey($"{symbol}"))
                        possibleTag.Append(symbol);
                }
                else
                    tokens.Remove("\\");
            }

            if (possibleTag.Length > 0)
            {
                var type = TokensBySeparator[possibleTag.ToString()];
                ProcessToken(possibleTag.ToString(), textToParse.Length - possibleTag.Length);
            }

            return result;
        }

        private void Initialize(string toParse)
        {
            tokens = new Dictionary<string, Token>();
            result = new List<Token>();
            textToParse = toParse;
        }

        private void ProcessToken(string separator, int index)
        {
            if (ExecuteScreening()) return;

            if (tokens.TryGetValue(separator, out var token) && IsCorrectTokenCloseIndex(index, textToParse))
            {
                token.Close(index);
                token.Accept(this);
                return;
            }

            token = TokensBySeparator[separator].Invoke(index);

            if (token.IsNonPaired)
                token.Accept(this);
            else if (IsCorrectTokenOpenIndex(index, textToParse, separator.Length))
                tokens[separator] = token;
        }

        internal void Visit(HeaderToken token)
        {
            if (token.OpenIndex != 0 && textToParse[token.OpenIndex - 1] != '\n')
                return;

            var closeIndex = textToParse.IndexOf('\n', token.OpenIndex);

            if (closeIndex == -1)
                closeIndex = textToParse.Length;

            token.Close(closeIndex);
            result.Add(token);
        }

        internal void Visit(BoldToken token)
        {
            token.ValidatePlacedCorrectly(textToParse);

            ValidateBoldTokenInteractions(token);

            tokens.Remove(token.GetSeparator());

            if (token.IsCorrect)
                result.Add(token);
        }

        internal void Visit(ItalicToken token)
        {
            token.ValidatePlacedCorrectly(textToParse);

            ValidateItalicTokenInteractions(token);

            tokens.Remove(token.GetSeparator());

            if (token.IsCorrect)
                result.Add(token);
        }

        internal void Visit(ScreeningToken token)
        {
            tokens.Add(token.GetSeparator(), token);
        }

        private bool ExecuteScreening()
        {
            if (!tokens.ContainsKey(ScreeningToken.Separator)) return false;

            tokens.Remove(ScreeningToken.Separator, out var token);
            token.Close(token.OpenIndex);
            result.Add(token);
            return true;
        }

        private void ValidateItalicTokenInteractions(ItalicToken token)
        {
            if (!tokens.TryGetValue(BoldToken.Separator, out var boldToken)) return;
            if (!token.IsIntersectWith(boldToken)) return;

            boldToken.IsCorrect = false;
            token.IsCorrect = false;
        }

        private void ValidateBoldTokenInteractions(BoldToken token)
        {
            if (!token.IsCorrect || !tokens.TryGetValue(ItalicToken.Separator, out var italicToken)) return;

            if (token.IsIntersectWith(italicToken))
            {
                italicToken.IsCorrect = false;
                token.IsCorrect = false;
            }

            if (italicToken.OpenIndex < token.OpenIndex && italicToken.IsOpened)
                token.IsCorrect = false;
        }

        private static bool IsCorrectTokenOpenIndex(int openIndex, string text, int length)
        {
            var indexNextToSeparator = openIndex + length;

            return openIndex != text.Length - 1 && indexNextToSeparator < text.Length &&
                   text[indexNextToSeparator] != ' ';
        }

        private static bool IsCorrectTokenCloseIndex(int closeIndex, string text)
        {
            return closeIndex != 0 && text[closeIndex - 1] != ' ';
        }
    }
}