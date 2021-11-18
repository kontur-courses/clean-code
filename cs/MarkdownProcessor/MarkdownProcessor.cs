using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownProcessor
{
    public class MarkdownProcessor
    {
        private Dictionary<string, Tuple<string>> tags;
        private List<Stack<string>> currentExtractors = new List<Stack<string>>();
        private Tokenizer tokenizer;

        public MarkdownProcessor(Dictionary<string, Tuple<string>> tags)
        {
            this.tags = tags;
            tokenizer = new Tokenizer(tags.Keys.ToHashSet());
        }

        public string Render(string input)
        {
            var builder = new StringBuilder();
            
            foreach (var token in tokenizer.GetTokens(input))
            {
                if (token.Type is TokenType.Tag)
                {
                    builder.Append(GetTag(token.Value));
                }
                else
                {
                    builder.Append(GetString(token.Value));
                }
            }

            return builder.ToString();
        }

        private string GetTag(string extractor)
        {
            throw new NotImplementedException();
        }

        private string GetString(string value)
        {
            throw new NotImplementedException();
        }
    }
}