﻿using System;
using System.Collections.Generic;
using Markdown.Tokenizer;

namespace Markdown.MdTokens
{
    public class MdTokenizer : ITokenizer
    {
        public IEnumerable<IToken> MakeTokens(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            throw new NotImplementedException();
        }
    }
}