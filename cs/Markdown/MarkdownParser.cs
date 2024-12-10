using Markdown.AstNodes;
using Markdown.Enums;
using Markdown.Tokens;

namespace Markdown;

public class MarkdownParser : IParser
{
    private const MarkdownTokenName Text = MarkdownTokenName.Text;
    private const MarkdownTokenName Bold = MarkdownTokenName.Bold;
    private const MarkdownTokenName Italic = MarkdownTokenName.Italic;
    private const MarkdownTokenName NewLine = MarkdownTokenName.NewLine;
    private const MarkdownTokenName Space = MarkdownTokenName.Space;

    public RootMarkdownNode Parse(List<Token> tokens)
    {
        var root = new RootMarkdownNode();
        ParseChildren(tokens, root, 0, tokens.Count);
        return root;
    }

    private void ParseChildren(List<Token> tokens, IMarkdownNodeWithChildren parent, int left, int right)
    {
        if (left < 0 || right > tokens.Count) return;
        if (left >= right) return;
        var index = left;
        while (index >= left && index < right)
        {
            var token = tokens[index];
            switch (token)
            {
                case TextToken:
                case SpaceToken:
                case NewLineToken:
                case NumberToken:
                    parent.Children.Add(new TextMarkdownNode(token.Value));
                    index++;
                    break;
                case HeadingToken:
                {
                    var heading = new HeadingMarkdownNode();
                    var next = FindIndexOfCloseHeadingToken(tokens, index);
                    ParseChildren(tokens, heading, index + 1, next == -1 ? right : next);
                    parent.Children.Add(heading);
                    index = next == -1 ? right : next;
                    break;
                }
                case ItalicToken:
                {
                    index = ParseItalicWithChildren(tokens, parent, index, right);
                    break;
                }
                case BoldToken:
                {
                    index = ParseBoldWithChildren(tokens, parent, index);
                    break;
                }
            }
        }
    }

    private int ParseItalicWithChildren(List<Token> tokens, IMarkdownNodeWithChildren parent, int start, int right)
    {
        var italic = new ItalicMarkdownNode();
        var next = FindIndexOfCloseItalicToken(tokens, start);

        if (next == -1 || next >= right)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            return start + 1;
        }

        if (parent is ItalicMarkdownNode)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            ParseChildren(tokens, parent, start + 1, next);
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            return next + 1;
        }

        if (TokenInWord(tokens, start) && TokenInWord(tokens, next) &&
            ContainsToken(tokens, MarkdownTokenName.Space, start, next))
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            for (var j = start + 1; j < next; j++) parent.Children.Add(new TextMarkdownNode(tokens[j].Value));
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            return next + 1;
        }

        ParseChildren(tokens, italic, start + 1, next);
        if (italic.Children.Count == 0)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground + MarkdownSymbols.Ground));
            return start + 2;
        }

        parent.Children.Add(italic);
        return next + 1;
    }

    private int ParseBoldWithChildren(List<Token> tokens, IMarkdownNodeWithChildren parent, int i)
    {
        var bold = new BoldMarkdownNode();
        var next = FindIndexOfCloseBoldToken(tokens, i);
        if (next == -1 || parent is ItalicMarkdownNode)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.DoubleGround));
            return i + 1;
        }

        var indexOfIntersection = FindIndexOfIntersection(tokens, i + 1, next);
        if (indexOfIntersection.start > 0)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.DoubleGround));
            ParseChildren(tokens, parent, i + 1, indexOfIntersection.start);
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            ParseChildren(tokens, parent, indexOfIntersection.start + 1, next);
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.DoubleGround));
            ParseChildren(tokens, parent, next + 1, indexOfIntersection.end);
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.Ground));
            return indexOfIntersection.end + 1;
        }

        ParseChildren(tokens, bold, i + 1, next);
        if (bold.Children.Count == 0)
        {
            parent.Children.Add(new TextMarkdownNode(MarkdownSymbols.DoubleGround + MarkdownSymbols.DoubleGround));
            return i + 2;
        }

        parent.Children.Add(bold);
        return next + 1;
    }

    private int FindIndexOfCloseItalicToken(List<Token> tokens, int start)
    {
        var index = start + 1;
        if (index < tokens.Count && tokens[index].Is(Space)) return -1;
        while (index < tokens.Count && tokens[index].Name != NewLine)
        {
            if (!tokens[index].Is(Italic))
            {
                index++;
                continue;
            }

            if (index + 1 < tokens.Count && tokens[index + 1].Is(Italic))
            {
                index += 2;
                continue;
            }

            if (index > 0 && !tokens[index - 1].Is(Space)) return index;
            index++;
        }

        return -1;
    }

    private int FindIndexOfCloseBoldToken(List<Token> tokens, int start)
    {
        var index = start + 1;
        if (index >= tokens.Count || tokens[index].Is(Space)) return -1;
        while (index < tokens.Count && tokens[index].Name != NewLine)
        {
            if (index > 0 && tokens[index].Is(Bold) && !tokens[index - 1].Is(Space))
                return index;
            index++;
        }

        return -1;
    }

    private int FindIndexOfCloseHeadingToken(List<Token> tokens, int start)
    {
        var index = start;
        while (index < tokens.Count && !tokens[index].Is(NewLine))
            index++;
        return index == tokens.Count ? -1 : index;
    }

    private (int start, int end) FindIndexOfIntersection(List<Token> tokens, int left, int right)
    {
        for (var i = left; i < right; i++)
            if (tokens[i] is ItalicToken)
            {
                var end = FindIndexOfCloseItalicToken(tokens, i);
                if (end > right) return (i, end);
                if (end == -1) continue;
                i = end + 1;
            }

        return (-1, -1);
    }

    private bool TokenInWord(List<Token> tokens, int index)
        => index > 0 && tokens[index - 1].Is(Text) && index + 1 < tokens.Count &&
           tokens[index + 1].Is(Text);

    private bool ContainsToken(List<Token> tokens, MarkdownTokenName expected, int left, int right)
    {
        for (var i = left; i < right; i++)
            if (tokens[i].Is(expected))
                return true;
        return false;
    }
}