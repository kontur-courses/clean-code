﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public class EscapeToken : IToken
    {
        public string Value => "\\";
        public TokenType Type => TokenType.Escape;
        public int Position { get; }
        public bool IsOpening => false;
        public bool ShouldBeSkipped { get; }
        public string OpeningTag => Value;
        public string ClosingTag => Value;
    }
}
