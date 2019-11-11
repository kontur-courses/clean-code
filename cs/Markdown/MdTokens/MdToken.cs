﻿using Markdown.Tokenizer;

namespace Markdown.MdTokens
{
    public class MdToken : IToken
    {
        public string Content { get; }
        public string SpecialSymbolBeginning { get; }
        public string SpecialSymbolEnding { get; }

        public MdToken(string content, string specialSymbolBeginning, string specialSymbolEnding)
        {
            Content = content;
            SpecialSymbolBeginning = specialSymbolBeginning;
            SpecialSymbolEnding = specialSymbolEnding;
        }
    }
}