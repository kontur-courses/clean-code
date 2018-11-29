using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Registers;

namespace Markdown
{
    public class Md
    {
        private readonly List<BaseRegister> blockRegisters, inlineRegisters;

        public Md()
        {
            blockRegisters = new List<BaseRegister>()
            {
                new ParagraphRegister(),
                new HorLineRegister(),
                new HeaderRegister()
            };
            
            inlineRegisters = new List<BaseRegister>()
            {
                new StrongRegister(),
                new EmphasisRegister(),
            };
        }

        public string Render(string input)
        {
            return TokensToHtml(ParseToTokens(input, blockRegisters));
        }

        private List<Token> ParseToTokens(string input, List<BaseRegister> registers)
        {
            var tokens = new List<Token>();
            var outsideChars = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var token = registers
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .OrderByDescending(t => t.Priority)
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

        private string TokensToHtml(List<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var value = token.IsParseInside
                    ? TokensToHtml(ParseToTokens(token.Value, inlineRegisters))
                    : token.Value;

                result.Append(token.OpenTag);
                result.Append(value);
                result.Append(token.CloseTag);
            }

            return result.ToString();
        }
    }
}