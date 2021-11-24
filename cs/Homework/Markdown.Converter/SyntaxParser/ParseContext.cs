using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.SyntaxParser.ConcreteParsers;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class ParseContext
    {
        public readonly Token[] Tokens;
        public int Position;

        public ParseContext(IEnumerable<Token> tokens) => Tokens = tokens.ToArray();

        public Token Current => Peek(0);
        public Token LookAhead => Peek(1);
        public Token Previous => Peek(-1);

        public IEnumerable<TokenTree> Parse()
        {
            while (Position != Tokens.Length)
            {
                yield return ParseToken();
                NextToken();
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
            var index = Position + offset;
            if (index >= Tokens.Length)
                return Tokens[^1];
            if (index <= 0)
                return Tokens[0];

            return Tokens[index];
        }

        public void NextToken() => Position++;

        public bool IsEndOfFileOrNewLine(int offset = 1) =>
            Peek(offset).TokenType == TokenType.NewLine || IsEndOfFile(offset);

        public bool IsEndOfFile(int offset = 1) => Position + offset == Tokens.Length;
    }
}