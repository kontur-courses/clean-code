﻿namespace Markdown
{
    public class TokenWithUnderScore: IToken
    {
        public string Value => "_";
        public TokenType TokenType { get; }
        
        public IToken Create(string[] text, int index) => 
            NextSymbolIsTheSame(index, text, "_") ? new TokenStrong(): new TokenItalics();

        private bool NextSymbolIsTheSame(int index, string[] text, string symbol) => 
            index + 1 < text.Length && symbol == text[index + 1];
    }
}