using System;
using Markdown.TagStore;

namespace Markdown
{
    public class Converter : IConverter
    {
        private ITokenizer tokenizer;

        public Converter(ITagStore from, ITagStore to, ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public string Convert(string text)
        {
            var tokens = tokenizer.Tokenize(text);
            throw new NotImplementedException();
        }
    }
}