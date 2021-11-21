using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class MarkdownSyntaxParser : ISyntaxParser
    {
        private readonly Dictionary<TokenType, TokenType> oppositeTokenTypes = new()
        {
            {TokenType.Italics, TokenType.Bold},
            {TokenType.Bold, TokenType.Italics}
        };

        private int position;
        private Token[] tokens;

        private Token Current => Peek(0);
        private Token LookAhead => Peek(1);
        private Token Previous => Peek(-1);

        public IEnumerable<TokenTree> Parse(IEnumerable<Token> lexedTokens)
        {
            tokens = lexedTokens?.ToArray() ?? throw new ArgumentNullException(nameof(lexedTokens));
            while (position != tokens.Length)
            {
                yield return ParseToken();
                NextToken();
            }
        }

        private Token Peek(int offset)
        {
            var index = position + offset;
            if (index >= tokens.Length)
                return tokens[^1];
            if (index <= 0)
                return tokens[0];

            return tokens[index];
        }

        private void NextToken()
        {
            position++;
        }

        private TokenTree ParseToken()
        {
            return Current.TokenType switch
            {
                TokenType.Text or TokenType.NewLine => TokenTree.FromText(Current.Value),
                TokenType.Italics => ParseItalics(),
                TokenType.Bold => ParseBold(),
                TokenType.Escape => new TokenTree(ParseEscape()),
                TokenType.Header1 => ParseHeader1(),
                _ => throw new ArgumentOutOfRangeException($"unknown token type: {Current.TokenType}")
            };
        }

        private TokenTree ParseItalics()
        {
            return ParseUnderscore(() =>
            {
                return Current.TokenType switch
                {
                    TokenType.Bold => TokenTree.FromText("__"),
                    _ => ParseToken()
                };
            });
        }

        private TokenTree ParseBold()
        {
            return ParseUnderscore(ParseToken);
        }

        private Token ParseEscape()
        {
            if (IsEndOfFile())
                return Token.Text("\\");

            switch (LookAhead.TokenType)
            {
                case TokenType.Bold:
                case TokenType.Italics:
                    NextToken();
                    return Token.Text(Current.Value);
                case TokenType.Escape:
                    NextToken();
                    break;
            }

            return Token.Text("\\");
        }

        private TokenTree ParseHeader1()
        {
            if (Previous.TokenType != TokenType.NewLine && position != 0)
                return TokenTree.FromText(Current.Value);
            if (position + 1 == tokens.Length)
                return TokenTree.FromText(string.Empty);
            var buffer = new List<TokenTree>();
            do
            {
                NextToken();
                buffer.Add(ParseToken());
            } while (!IsEndOfFileOrNewLine());

            return new TokenTree(Token.Header1, buffer.ToArray());
        }

        private TokenTree ParseUnderscore(Func<TokenTree> transformCurrentItem)
        {
            var current = Current;
            if (ShouldParseAsText())
                return TokenTree.FromText(Current.Value);
            var intersectionOffset = IsIntersectsWith(oppositeTokenTypes[current.TokenType]);
            if (intersectionOffset != -1)
                return ParseToText(intersectionOffset, Current.Value);
            var buffer = new List<TokenTree>();
            do
            {
                NextToken();
                if (Current.TokenType == current.TokenType)
                    return new TokenTree(Current, buffer.ToArray());
                buffer.Add(transformCurrentItem());
            } while (!IsEndOfFileOrNewLine());

            return TokenTree.FromText(buffer.Aggregate(current.Value, (s, token) => s + token.Token.Value));
        }

        private bool ShouldParseAsText()
        {
            if (IsEndOfFileOrNewLine())
                return true;
            if (SurroundedWithDigits())
                return true;
            if (char.IsWhiteSpace(LookAhead.Value.First()))
                return true;
            if (LookAhead.TokenType == Current.TokenType)
                return true;
            if (!HasTokenInSameLine())
                return true;
            if (ContainsWordsThatSeparatedBy(' '))
                return true;
            return false;
        }

        private bool SurroundedWithDigits()
        {
            return SurroundedWithDigits(Previous, LookAhead) || SurroundedWithDigits(LookAhead, Previous);
        }

        private bool SurroundedWithDigits(Token before, Token after)
        {
            return char.IsDigit(before.Value.First()) && !char.IsWhiteSpace(after.Value.Last());
        }

        private bool ContainsWordsThatSeparatedBy(char symbol)
        {
            var i = 1;
            Token currentToken;
            do
            {
                currentToken = Peek(i);
                var value = currentToken.Value;
                if (value.Contains(symbol))
                    return true;
                i++;
            } while (!IsEndOfFileOrNewLine() && currentToken.TokenType != Current.TokenType);

            return false;
        }

        private TokenTree ParseToText(int endOffset, string prefix)
        {
            var buffer = new List<TokenTree>();
            for (var i = 0; i < endOffset; i++)
            {
                NextToken();
                buffer.Add(TokenTree.FromText(Current.Value));
            }

            return TokenTree.FromText(buffer.Aggregate(prefix, (s, token) => s + token.Token.Value));
        }

        private int IsIntersectsWith(TokenType tokenType)
        {
            var closingTag = GetOffsetOfFirstTagAppearanceInLine(Current.TokenType);
            var otherOpeningTag = GetOffsetOfFirstTagAppearanceInLine(tokenType);
            if (otherOpeningTag == -1)
                return -1;
            var otherClosingTag = GetOffsetOfFirstTagAppearanceInLine(tokenType, otherOpeningTag + 1);
            var result = otherOpeningTag < closingTag && otherClosingTag > closingTag;
            return result ? otherClosingTag : -1;
        }

        private bool HasTokenInSameLine()
        {
            return GetOffsetOfFirstTagAppearanceInLine(Current.TokenType) != -1;
        }

        private int GetOffsetOfFirstTagAppearanceInLine(TokenType tokenType, int startOffset = 1)
        {
            var i = startOffset;
            Token currentToken;
            do
            {
                currentToken = Peek(i);
                if (currentToken.TokenType == tokenType && CanBeClosedTag(i))
                    return i;
                i++;
            } while (currentToken.TokenType != TokenType.NewLine && position + i != tokens.Length);

            return -1;
        }

        private bool IsEndOfFileOrNewLine(int offset = 1)
        {
            return Peek(offset).TokenType == TokenType.NewLine || IsEndOfFile(offset);
        }

        private bool IsEndOfFile(int offset = 1)
        {
            return position + offset == tokens.Length;
        }

        private bool CanBeClosedTag(int offset)
        {
            return !char.IsWhiteSpace(Peek(offset - 1).Value.Last())
                   || IsEndOfFileOrNewLine(offset + 1);
        }
    }
}