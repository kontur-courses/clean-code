using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class MdParser : IMdParser
    {
        private ParserContext ParserContext { get; set; }
        
        public readonly IReadOnlyDictionary<string, Func<int, Token>> TokensBySeparator;
        public string TextToParse => ParserContext.TextToParse;
        public IReadOnlyDictionary<string, Token> Tokens => ParserContext.Tokens;
        public IReadOnlyList<Token> Result => ParserContext.Result;

        public MdParser(IReadOnlyDictionary<string, Func<int, Token>> tokensBySeparator)
        {
            TokensBySeparator = tokensBySeparator;
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

                var tag = possibleTag.ToString();

                if (TokensBySeparator.ContainsKey(tag))
                    ProcessToken(tag, i - tag.Length);

                possibleTag.Clear();

                if (TokensBySeparator.Keys.Any(x => x.StartsWith(symbol)))
                    possibleTag.Append(symbol);
            }

            if (possibleTag.Length > 0)
                ProcessToken(possibleTag.ToString(), TextToParse.Length - possibleTag.Length);

            return ParserContext.Result;
        }

        public void AddScreening(ScreeningToken token)
        {
            ParserContext.Tokens.Add(token.GetSeparator(), token);
        }

        private void ProcessToken(string separator, int index)
        {
            if (ExecuteScreening(index))
            {
                separator = separator[1..];
                index++;

                if (!(separator.Length > 0 && TokensBySeparator.ContainsKey(separator)))
                    return;
            }

            if (ParserContext.Tokens.Remove(separator, out var token) && Token.IsCorrectTokenCloseIndex(index, TextToParse))
            {
                token.Close(index);

                if (token.Validate(this))
                    ParserContext.Result.Add(token);

                return;
            }

            token = TokensBySeparator[separator].Invoke(index);

            if (token.IsNonPaired && token.Validate(this))
                ParserContext.Result.Add(token);
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