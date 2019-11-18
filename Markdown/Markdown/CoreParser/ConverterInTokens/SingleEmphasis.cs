using System;
using System.Collections.Generic;
using Markdown.ConverterInTokens;
using Markdown.Tokens;

namespace Markdown.ConverterTokens
{
    public class SingleEmphasis : AbstractConverterInToken
    {
        public SingleEmphasis() : base("_", "_")
        {
        }
        
        public override IToken GetCurrentToken(string Text, int startIndex, IToken[] nestedTokens)
        {
            return new SingleEmphasisToken(Text, startIndex, nestedTokens);
        }
    }

 }