using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownProcessor
{
    public class MarkdownProcessor
    {
        private Dictionary<string, Tuple<string>> tags;
        private Tokenizer tokenizer;

        public MarkdownProcessor(Dictionary<string, Tuple<string>> tags)
        {
            this.tags = tags;
            tokenizer = new Tokenizer(tags.Keys.ToHashSet());
        }

        public string Render(string input)
        {
            throw new NotImplementedException();
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