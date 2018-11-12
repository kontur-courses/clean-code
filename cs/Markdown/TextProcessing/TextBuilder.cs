using System;
using System.Collections.Generic;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TextBuilder
    {
        public List<Token> Tokens { get; set; }

        public TextBuilder(List<Token> tokens)
        {
            Tokens = tokens;
        }
        public string BuildText()
        {
            throw new NotImplementedException();
        }
    }
}