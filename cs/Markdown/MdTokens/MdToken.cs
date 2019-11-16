﻿using Markdown.Tokenizer;

namespace Markdown.MdTokens
{
    public class MdToken : IToken
    {
        public string Content { get; }
        public string BeginningSpecialSymbol { get; }
        public string EndingSpecialSymbol { get; }

        public MdToken(string content, string beginningSpecialSymbol, string endingSpecialSymbol)
        {
            Content = content;
            BeginningSpecialSymbol = beginningSpecialSymbol;
            EndingSpecialSymbol = endingSpecialSymbol;
        }
    }
}