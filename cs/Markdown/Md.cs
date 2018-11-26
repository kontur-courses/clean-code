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
            var resHtmlText = new StringBuilder();

            foreach (var tag in GetBlockTags(input))
            {
                resHtmlText.Append(tag.OpenTag);
                resHtmlText.Append(ParseInternalTags(tag.Value));
                resHtmlText.Append(tag.CloseTag);
            }

            return resHtmlText.ToString();
        }

        private List<Token> GetBlockTags(string input)
        {
            var tags = new List<Token>();

            for (var i = 0; i < input.Length; i++)
            {
                var token = registers
                    .Where(r => r.IsBlockRegister)
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .OrderByDescending(t => t.Shift)
                        .ThenByDescending(t => t.Priority)
                    .FirstOrDefault();

                if (token != null)
                {
                    tags.Add(token);
                    i += token.Shift - 1;
                }
            }

            return tags;
        }

        private string ParseInternalTags(string input)
        {
            var restHtmlText = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var token = registers
                    .Where(r => !r.IsBlockRegister)
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .OrderByDescending(t => t.Shift)
                    .ThenByDescending(t => t.Priority)
                    .FirstOrDefault();

                if (token != null)
                {
                    restHtmlText.Append(token.OpenTag);
                    restHtmlText.Append(ParseInternalTags(token.Value));
                    restHtmlText.Append(token.CloseTag);

                    i += token.Shift - 1;
                }
                else
                {
                    restHtmlText.Append(input[i]);
                }
            }

            return restHtmlText.ToString();
        }
    }
}