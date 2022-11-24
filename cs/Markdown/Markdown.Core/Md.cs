using System.Text;
using Markdown.Core.Entities;
using Markdown.Core.Entities.TagMaps;
using Markdown.Core.Entities.Abstract;

namespace Markdown.Core
{
    public class Md
    {
        private readonly List<BaseTag> _blockRegisters, _inlineRegisters;

        public Md()
        {
            _blockRegisters = new List<BaseTag>()
            {
                new PTag(),
                new HeaderTag()
            };

            _inlineRegisters = new List<BaseTag>()
            {         
                new StrongTag(),
                new EmTag(),
            };
        }

        public string Render(string input)
        {
            return TokensToHtml(ParseToTokens(input, _blockRegisters));
        }

        private List<Token> ParseToTokens(string input, IReadOnlyCollection<BaseTag> registers)
        {
            var tokens = new List<Token>();
            var outsideChars = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var token = registers
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .MaxBy(t => t.Priority);

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
                else { outsideChars.Append(input[i]); }
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
                    ? TokensToHtml(ParseToTokens(token.Value, _inlineRegisters))
                    : token.Value;

                result.Append(token.OpenTag);
                result.Append(value);
                result.Append(token.CloseTag);
            }

            return result.ToString();
        }
    }
}