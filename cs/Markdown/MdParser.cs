using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class MdParser
    {
        public static readonly Dictionary<string, Type> Separators = new()
        {
            { ItalicToken.Separator, typeof(ItalicToken) },
            { BoldToken.Separator, typeof(BoldToken) },
            { HeaderToken.Separator, typeof(HeaderToken) }
        };

        internal int CurrentIndex;
        internal List<Token> Result;
        internal string TextToParse;
        internal Dictionary<Type, Token> Tokens;

        private void Initialize(string toParse)
        {
            Tokens = new Dictionary<Type, Token>();
            Result = new List<Token>();
            TextToParse = toParse;
            CurrentIndex = 0;
        }

        public IEnumerable<Token> ParseTokens(string input)
        {
            Initialize(input);
            var possibleTag = new StringBuilder();

            for (; CurrentIndex < TextToParse.Length; CurrentIndex++)
            {
                var symbol = TextToParse[CurrentIndex];

                if (symbol == '\\')
                {
                    CurrentIndex++;
                    possibleTag.Clear();
                    continue;
                }

                if (Separators.ContainsKey($"{possibleTag}{symbol}"))
                    possibleTag.Append(symbol);
                else
                {
                    if (Separators.ContainsKey(possibleTag.ToString()))
                    {
                        var type = Separators[possibleTag.ToString()];
                        ProcessToken(type, CurrentIndex - possibleTag.Length + 1);
                    }

                    possibleTag.Clear();
                }
            }

            return Result;
        }

        private void ProcessToken(Type type, int index)
        {
            Token token = null;

            if (!Tokens.ContainsKey(type) && IsCorrectOpenPosition(index, TextToParse))
                token = (Token)Activator.CreateInstance(type, index);
            else if (Tokens[type].IsOpened && IsCorrectClosePosition(index, TextToParse)) 
                token = Tokens[type];

            token?.Accept(this);
        }

        private static bool IsCorrectOpenPosition(int openIndex, string text)
        {
            return openIndex != text.Length - 1 && text[openIndex - 1] != ' ';
        }

        private static bool IsCorrectClosePosition(int closeIndex, string text)
        {
            return closeIndex != 0 && text[closeIndex - 1] != ' ';
        }

        internal void Handle(HeaderToken token)
        {
            if (token.OpenIndex != 0 && TextToParse[token.OpenIndex - 1] != '\n')
                return;

            var closeIndex = TextToParse.IndexOf('\n');

            if (closeIndex == -1)
                closeIndex = TextToParse.Length - 1;

            token.CloseIndex = closeIndex;
            Result.Add(token);
        }

        internal void Handle(ItalicToken token)
        {
            throw new NotImplementedException();
        }

        internal void Handle(BoldToken token)
        {
            throw new NotImplementedException();
        }
    }
}