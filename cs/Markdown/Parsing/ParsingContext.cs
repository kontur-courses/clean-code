using Markdown.Parsing.Nodes;
using Markdown.Parsing.Rules;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing;

public class ParsingContext
{
    public List<Token> Tokens { get; set; }
    public int Index { get; set; }
    public List<MarkdownNode> Nodes { get; set; } = [];
    public Stack<UnderscoreInfo> OpenedUnderscores { get; set; } = new();

    public Token CurrentToken => Tokens[Index];
    public Token? PeekToken(int offset = 1) => Index + offset < Tokens.Count ? Tokens[Index + offset] : null;
    public Token? PreviousToken => Index > 0 ? Tokens[Index - 1] : null;
    public void MoveForward(int count = 1) => Index += count;
    public bool IsEndOfTokens => Index >= Tokens.Count;
}