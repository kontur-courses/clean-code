using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Registers;

namespace Markdown
{
    public class Md
    {
        private readonly List<BaseRegister> registers;

        public Md()
        {
            registers = new List<BaseRegister>
            {
                new ParagraphRegister(),
                new HorLineRegister(),
                new StrongRegister(),
                new EmphasisRegister()
            };
        }

        public string Render(string input)
        {
            return tokensToHtml(parseToTokens(input, isBlockRegisters:true));
        }

        private List<Token> parseToTokens(string input, bool isBlockRegisters)
        {
            var tokens = new List<Token>();
            var outsideChars = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var token = registers
                    .Where(r => r.IsBlockRegister == isBlockRegisters)
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .OrderByDescending(t => t.Shift)
                    .ThenByDescending(t => t.Priority)
                    .FirstOrDefault();

                if (token != null)
                {
                    if (outsideChars.Length != 0)
                    {
                        tokens.Add(new Token(outsideChars.ToString(), "", "", 0, outsideChars.Length, false));
                        outsideChars.Clear();
                    }

                    tokens.Add(token);
                    i += token.Shift - 1;
                }
                else
                    outsideChars.Append(input[i]);
            }

            if (outsideChars.Length != 0)
                tokens.Add(new Token(outsideChars.ToString(), "", "", 0, outsideChars.Length, false));

            return tokens;
        }

        private string tokensToHtml(List<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var value = token.IsParseInside
                    ? tokensToHtml(parseToTokens(token.Value, false))
                    : token.Value;

                result.Append(token.OpenTag);
                result.Append(value);
                result.Append(token.CloseTag);
            }

            return result.ToString();
        }
    }
}