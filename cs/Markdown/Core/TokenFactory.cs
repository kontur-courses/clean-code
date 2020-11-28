using System;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class TokenFactory
    {
        public static IToken CreateNewToken(string markdownTag, string tokenSource, int startIndex) => markdownTag switch
        {
            BoldToken.MdTag => new BoldToken(tokenSource, startIndex),
            ItalicToken.MdTag => new ItalicToken(tokenSource, startIndex),
            LinkToken.MdTag => new LinkToken(tokenSource, startIndex),
            _ => throw new ArgumentException($"{markdownTag} is not supported Markdown tag")    
        };
    }
}