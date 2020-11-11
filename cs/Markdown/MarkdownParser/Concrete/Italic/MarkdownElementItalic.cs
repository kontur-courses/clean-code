﻿using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Italic
{
    public class MarkdownElementItalic : MarkdownElement
    {
        public MarkdownElementItalic(Token[] tokens) : base(tokens)
        {
            Content = tokens.Skip(1).SkipLast(1).ToArray();
        }

        public MarkdownElementItalic(Token startToken, ICollection<Token> content, Token endToken)
            : base(content.Prepend(startToken).Append(endToken).ToArray())
        {
            Content = content;
        }

        public ICollection<Token> Content { get; }
    }
}