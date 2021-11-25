using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.SyntaxParser.ConcreteParsers;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class ParseContext
    {
        private readonly Token[] tokens;
        private int position;

        public ParseContext(IEnumerable<Token> tokens) => this.tokens = tokens.ToArray();

        public Token Current => Peek(0);
        public Token LookAhead => Peek(1);
        public Token Previous => Peek(-1);

        public IEnumerable<TokenTree> Parse()
        {
            while (position != tokens.Length)
            {
                yield return ParseToken();
                MoveToNextToken();
            }
        }

        public TokenTree ParseToken()
        {
            return Current.TokenType switch
            {
                TokenType.Text or TokenType.NewLine => new TextParser(this).Parse(),
                TokenType.Italics => new ItalicsParser(this).Parse(),
                TokenType.Bold => new BoldParser(this).Parse(),
                TokenType.Escape => new EscapeParser(this).Parse(),
                TokenType.Header1 => new Header1Parser(this).Parse(),
                TokenType.OpenImageAlt => new ImageParser(this).Parse(),
                _ => throw new Exception($"unknown token type: {Current.TokenType}")
            };
        }

        public Token Peek(int offset)
        {
            var index = position + offset;
            if (index >= tokens.Length)
                return tokens[^1];
            if (index <= 0)
                return tokens[0];

            return tokens[index];
        }

        public void MoveToNextToken() => position++;

        public bool IsEndOfFileOrNewLine(int offset = 1) =>
            Peek(offset).TokenType == TokenType.NewLine || IsEndOfFile(offset);

        public bool IsStartOfFileOrNewLine(int offset = 0) =>
            Peek(offset - 1).TokenType == TokenType.NewLine || position + offset == 0;

        public bool IsEndOfFile(int offset = 1) => position + offset == tokens.Length;
    }
}