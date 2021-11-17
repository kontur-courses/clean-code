using System;
using Markdown.TagStore;

namespace Markdown
{
    public class Tokenizer
    {
        private ITokenDetector detector;

        public Tokenizer(ITokenDetector detector)
        {
            this.detector = detector;
        }

        public Token[] Tokenize(string text)
        {
            var nextToken = detector.GetNextToken(0, text);
            throw new NotImplementedException();
        }
    }
}