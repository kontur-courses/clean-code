using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownProcessor : IMarkdownProcessor
    {
        public List<Mark> Marks { get; }
        public MarkdownProcessor()
        {
            ///заполняется тэгами для форматирования токенов
        }
        public List<TokenMd> FormatTokens(List<TokenMd> tokens)
        {
            ///токены форматируются с использованием функций тэгов
            throw new NotImplementedException();
        }
        private TokenMd FormatToken(TokenMd token)
        {
            ///токены форматируются с использованием функций тэгов
            throw new NotImplementedException();
        }
        
    }
}