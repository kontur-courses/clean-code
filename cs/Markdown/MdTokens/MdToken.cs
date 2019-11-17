﻿using Markdown.Tokenizer;

namespace Markdown.MdTokens
{
    public class MdToken : IToken
    {
        public string Content { get; set; }
        public string BeginningSpecialSymbol { get; set; }
        public string EndingSpecialSymbol { get; set; }

        public MdToken(string content, string beginningSpecialSymbol, string endingSpecialSymbol)
        {
            Content = content;
            BeginningSpecialSymbol = beginningSpecialSymbol;
            EndingSpecialSymbol = endingSpecialSymbol;
        }
    }
}