using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownProcessor
    {
        private Dictionary<string, Tuple<string>> tags;
        private List<Stack<string>> currentExtractors = new List<Stack<string>>();

        public MarkdownProcessor(Dictionary<string, Tuple<string>> tags)
        {
            this.tags = tags;
        }

        public string Render(string input)
        {
            throw new NotImplementedException();
        }

        private string GetTag(string extractor)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<string> GetTextOrExtractor()
        {
            throw new NotImplementedException();
        }
    }
}