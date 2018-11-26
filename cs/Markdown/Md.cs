using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly List<IReadable> blockReaders;
        private readonly List<IReadable> inlineReaders;

        public Md()
        {
            blockReaders = new List<IReadable> {new ParagraphRegister(),
                                                new HorLineRegister()};
            inlineReaders = new List<IReadable> {new StrongRegister(),
                                                 new EmphasisRegister()};
        }

        public string Render(string input)
        {
            StringBuilder htmlText = new StringBuilder();
            List<Token> blockTags = GetBlockTags(input);        // Гуд

            foreach (var tag in blockTags)
            {
                htmlText.Append(tag.OpenTag);
                htmlText.Append(ParseToHtml(tag.Value));
                htmlText.Append(tag.CloseTag);
            }
            return htmlText.ToString();
        }

        private List<Token> GetBlockTags(string input)
        {
            List<Token> tags = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                var token = blockReaders
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
            StringBuilder htmlText = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var token = inlineReaders
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
                    htmlText.Append(input[i]);
            }
            return htmlText.ToString();
        }
    }
}