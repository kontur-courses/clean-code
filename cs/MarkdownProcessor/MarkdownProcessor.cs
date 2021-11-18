using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownProcessor
{
    public class MarkdownProcessor
    {
        private Dictionary<string, Tuple<string>> tags;
        private Dictionary<string, Stack<string>> currentExtractors = new Dictionary<string, Stack<string>>();
        private Tokenizer tokenizer;

        public MarkdownProcessor(Dictionary<string, Tuple<string>> tags)
        {
            this.tags = tags;
            tokenizer = new Tokenizer();
        }

        public string Render(string input)
        {
            var builder = new StringBuilder();
            
            foreach (var token in tokenizer.GetTokens(input))
            {
                if (token.Type is TokenType.Text)
                {
                    builder.Append(GetString(token.Value));
                    
                }
                else
                {
                    builder.Append(GetTag(token.Value));
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