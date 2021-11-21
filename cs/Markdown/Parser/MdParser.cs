using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class MdParser
    {
        public readonly Dictionary<string, Func<int, Token>> TokensBySeparator = new()
        {
            { ItalicToken.Separator, index => new ItalicToken(index) },
            { BoldToken.Separator, index => new BoldToken(index) },
            { HeaderToken.Separator, index => new HeaderToken(index) },
            { ScreeningToken.Separator, index => new ScreeningToken(index) },
            { ImageToken.Separator, index => new ImageToken(index) }
        };

        public ParserContext ParserContext { get; private set; }

        public string TextToParse => ParserContext.TextToParse;

        public void AddPossibleToken(string separator, Func<int, Token> tokenInstanceCreator)
        {
            if (TokensBySeparator.ContainsKey(separator))
                throw new InvalidOperationException("This separator is already present");

            TokensBySeparator.Add(separator, tokenInstanceCreator);
        }

        public IEnumerable<Token> ParseTokens(string textToParse)
        {
            ParserContext = new ParserContext(textToParse);
            var possibleTag = new StringBuilder();

            for (var i = 0; i < TextToParse.Length; i++)
            {
                var symbol = TextToParse[i];

                if (symbol == '\n')
                {
                    ParserContext.Tokens.Clear();
                    possibleTag.Clear();
                    continue;
                }

                if (TokensBySeparator.Keys.Any(x => x.StartsWith($"{possibleTag}{symbol}")))
                {
                    possibleTag.Append(symbol);
                    continue;
                }

                if (TokensBySeparator.ContainsKey(possibleTag.ToString()))
                    ProcessToken(possibleTag.ToString(), i - possibleTag.Length);

                possibleTag.Clear();

                if (TokensBySeparator.Keys.Any(x => x.StartsWith(symbol)))
                    possibleTag.Append(symbol);
            }

            if (possibleTag.Length > 0)
                ProcessToken(possibleTag.ToString(), TextToParse.Length - possibleTag.Length);

            return ParserContext.Result;
        }

        private void ProcessToken(string separator, int index)
        {
            if (ExecuteScreening(index)) return;

            if (ParserContext.Tokens.TryGetValue(separator, out var token) && Token.IsCorrectTokenCloseIndex(index, TextToParse))
            {
                token.Close(index);
                token.Accept(this);
                return;
            }

            token = TokensBySeparator[separator].Invoke(index);

            if (token.IsNonPaired)
                token.Accept(this);
            else if (Token.IsCorrectTokenOpenIndex(index, TextToParse, separator.Length))
                ParserContext.Tokens[separator] = token;
        }

        private bool ExecuteScreening(int index)
        {
            if (!ParserContext.Tokens.TryGetValue(ScreeningToken.Separator, out var token))
                return false;

            if (token.CloseIndex > index - ScreeningToken.Separator.Length)
                return true;

            if (token.CloseIndex != index - ScreeningToken.Separator.Length)
            {
                if (token.CloseIndex <= index)
                    ParserContext.Tokens.Remove(ScreeningToken.Separator);

                return false;
            }

            if (token.OpenIndex == token.CloseIndex)
                ParserContext.Result.Add(token);

            return true;
        }
    }
}