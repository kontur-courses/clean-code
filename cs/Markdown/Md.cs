using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Registers;

namespace Markdown
{
    public class Md
    {
        private readonly List<BaseReader> readers;

        public Md()
        {
            readers = new List<BaseReader>
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
            var blockTags = GetBlockTags(input); 

            foreach (var tag in blockTags)
            {
                resHtmlText.Append(tag.OpenTag);
                resHtmlText.Append(ParseToHtml(tag.Value));
                resHtmlText.Append(tag.CloseTag);
            }

            return resHtmlText.ToString();
        }

        private List<Token> GetBlockTags(string input)
        {
            var tags = new List<Token>();

            for (var i = 0; i < input.Length; i++)
            {
                var token = readers
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

        private string ParseToHtml(string input)
        {
            var htmlText = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var token = readers
                    .Where(r => !r.IsBlockRegister)
                    .Select(r => r.TryGetToken(input, i))
                    .Where(t => t != null)
                    .OrderByDescending(t => t.Shift)
                    .ThenByDescending(t => t.Priority)
                    .FirstOrDefault();

                if (token != null)
                {
                    htmlText.Append(token.OpenTag);
                    htmlText.Append(ParseToHtml(token.Value));
                    htmlText.Append(token.CloseTag);

                    i += token.Shift - 1;
                }
                else
                {
                    htmlText.Append(input[i]);
                }
            }

            return htmlText.ToString();
        }
    }
}