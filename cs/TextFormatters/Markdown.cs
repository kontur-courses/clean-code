using System;
using System.Collections.Generic;
using System.Text;

namespace TextFormatters
{
    public class Markdown : ITextFormatter
    {
        public string Format(string text)
        {
            var tokenizer = new Tokenizer(text);
            foreach (var token in tokenizer.Tokens())
            {
                var line = token.GetLine();
                var startToken = line.StartToken;
                var endToken = line.EndToken;

            }
            return text;
        }
    }
}
