using System;
using Markdown.TagStore;

namespace Markdown
{
    public class Converter
    {
        private Tokenizer tokenizer;
        
        public Converter(ITagStore from, ITagStore to)
        {
            tokenizer = new Tokenizer(new TokenDetector(from));
        }

        public string Convert(string text)
        {
            var tokens = tokenizer.Tokenize(text);
            throw new NotImplementedException();
        }
    }
}