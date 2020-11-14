using System;

namespace Markdown
{
    public class Md : IMarkupProcessor
    {
        private Tokenizer tokenizer;
        private string tags
        
        public Md()
        {
            tokenizer = new Tokenizer();
        }

        public string GetHtmlMarkup(string text)
        {
            var words = tokenizer.GetWords(text);
            return text;
        }
    }
}