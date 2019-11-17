using System;
using System.Collections.Generic;
using System.Text;
using static Markdown.ControlSymbols;

namespace Markdown
{
    public class Md
    {
        public static string Render(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            var tokens = new List<Token>();
            var position = 0;
            while (position < input.Length)
            {
                tokens.Add(IsControlSymbol(input, position)
                    ? TokenReader.ReadUntil(input, position)
                    : TokenReader.ReadWhile(AnalyzeSymbol, input, position));
                position = tokens[tokens.Count - 1].ActualEnd;
            }

            var stringHTML = new StringBuilder();
            foreach (var token in tokens)
                stringHTML.Append(token.ConvertToHTMLTag());

            return stringHTML.ToString();
        }
    }
}