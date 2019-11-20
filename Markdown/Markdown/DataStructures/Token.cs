using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public readonly int StartIndex;
        public int EndIndex { get; set; }
        public readonly MarkTranslatorElement MarkToTranslate;
        public string Value { get; set; }
        public readonly Token ParentToken;
        public List<Token> ChildTokens = new List<Token>();

        public Token(int startIndex, MarkTranslatorElement markToTranslate, Token parentToken = null)
        {
            StartIndex = startIndex;
            MarkToTranslate = markToTranslate;
            ParentToken = parentToken;
        }

        public override string ToString()
        {
            return $"{MarkToTranslate.To.Opening}{Value}{MarkToTranslate.To.Closing} {StartIndex}:{EndIndex}";
        }
    }
}