using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes;

public interface IPairingRule
{
    bool CanHandle(Token token);
    bool CanPair(PairedToken open, PairedToken close, IReadOnlyList<Token> content);
    bool AllowsChild(IPairingRule child) => true;
    TextNode Create(IReadOnlyList<TextNode> children);
}

public sealed class StrongRule : IPairingRule
{
    public bool CanHandle(Token token) => token.Type == TokenType.DoubleUnderscore;

    public bool CanPair(PairedToken open, PairedToken close, IReadOnlyList<Token> content)
    {
        return true;
    }

    public TextNode Create(IReadOnlyList<TextNode> children) => new Strong(children);
}

public sealed class EmRule : IPairingRule
{
    public bool CanHandle(Token token) => token.Type == TokenType.Underscore;

    public bool CanPair(PairedToken open, PairedToken close, IReadOnlyList<Token> content)
    {
        return true;
    }

    public bool AllowsChild(IPairingRule child) => false;

    public TextNode Create(IReadOnlyList<TextNode> children)
    {
        var sb = new StringBuilder();
        foreach (var child in children)
        {
            if (child is not Text text)
                throw new InvalidOperationException($"Emphasis can contain only text, got {child.GetType().Name}");
            sb.Append(text.Value);
        }

        return new Emphasis(sb.ToString());
    }
}



public interface IBlockRule
{
    bool Matches(IReadOnlyList<Token> line);
    IReadOnlyList<Token> StripMarker(IReadOnlyList<Token> line);
    BlockNode Create(IReadOnlyList<TextNode> inlineNodes);
}

public sealed class HeaderRule : IBlockRule
{
    public bool Matches(IReadOnlyList<Token> line) => line is [HeaderMarkerToken, ..];
    public IReadOnlyList<Token> StripMarker(IReadOnlyList<Token> line) => [.. line.Skip(1)];
    public BlockNode Create(IReadOnlyList<TextNode> inlineNodes) => new HeaderBlock(inlineNodes);
}

public sealed class ParagraphRule : IBlockRule
{
    public bool Matches(IReadOnlyList<Token> line) => true;
    public IReadOnlyList<Token> StripMarker(IReadOnlyList<Token> line) => line;
    public BlockNode Create(IReadOnlyList<TextNode> inlineNodes) => new TextBlock(inlineNodes);
}