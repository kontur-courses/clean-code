using System;
using System.Collections.Generic;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TextSplitter
    {
        public string Content { get; set; }

        public TextSplitter(string content)
        {
            Content = content;
        }

        public List<Token> SplitToTokens()
        {
            throw new NotImplementedException();
        }
    }
}