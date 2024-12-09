using System.Net.NetworkInformation;
using Markdown.Tokens;
using Markdown.Tokens.HtmlToken;
using Markdown.Tokens.HtmlToken.Header;
using Markdown.Tokens.HtmlToken.Italic;
using Markdown.Tokens.HtmlToken.ListItem;
using Markdown.Tokens.HtmlToken.Strong;
using Markdown.Tokens.HtmlToken.UnorderedList;
using Markdown.Tokens.StringToken;

namespace Markdown.Maker;

public class HtmlMaker : IMaker<RootToken>
{
    private Stack<BaseHtmlToken> rootChildren = new Stack<BaseHtmlToken>();
    private Stack<(int pos, BaseHtmlToken token)> temporaryStack = new Stack<(int, BaseHtmlToken)>();
    private StringToken[] tokens;
    private int position;


    private StringToken Peek(int offset)
    {
        var index = position + offset;
        if (index >= tokens.Length)
            return tokens[^1];
        return index < 0 ? tokens[0] : tokens[index];
    }

    private StringToken Next => Peek(1);
    private StringToken Current => Peek(0);
    private StringToken Previous => Peek(-1);

    public RootToken MakeFromTokens(IEnumerable<StringToken> input)
    {
        tokens = input.ToArray();
        List<BaseHtmlToken>? children;
        while (position < tokens.Length)
        {
            switch (Current.Type)
            {
                case StringTokenType.Unexpected:
                case StringTokenType.Text:
                    rootChildren.Push(new TextToken(Current.Value));
                    break;
                case StringTokenType.WhiteSpace:
                    var unclosedTags = new Stack<(int, BaseHtmlToken)>();
                    while (temporaryStack.Count > 0)
                    {
                        var (nodeTokenIndex, node) = temporaryStack.Pop();
                        switch (node)
                        {
                            case ItalicOpenToken:
                            case StrongOpenToken:
                                if ((nodeTokenIndex == 0 ||
                                     tokens[nodeTokenIndex - 1].Type == StringTokenType.WhiteSpace) &&
                                    tokens[nodeTokenIndex + 1].Type != StringTokenType.WhiteSpace)
                                    unclosedTags.Push((nodeTokenIndex, node));
                                break;
                            default:
                                unclosedTags.Push((nodeTokenIndex, node));
                                break;
                        }
                    }

                    foreach (var tag in unclosedTags)
                        temporaryStack.Push(tag);
                    rootChildren.Push(new TextToken(Current.Value));
                    break;
                case StringTokenType.NewLine:
                    if (temporaryStack.Any(pair => pair.token is HeaderOpenToken))
                    {
                        temporaryStack.Clear();
                        rootChildren.Push(new HeaderCloseToken(Current.Value));
                        children = UnitTokenEnds<HeaderOpenToken>().ToList();
                        rootChildren.Push(new HeaderToken(children));
                        rootChildren.Push(new TextToken(Current.Value));
                        break;
                    }

                    if (temporaryStack.Any(pair => pair.token is ListItemOpenToken))
                    {
                        temporaryStack.Clear();
                        rootChildren.Push(new ListItemCloseToken(Current.Value));
                        children = UnitTokenEnds<ListItemOpenToken>().ToList();
                        rootChildren.Push(new ListItemToken(children));
                        rootChildren.Push(new TextToken(Current.Value));
                        break;
                    }

                    rootChildren.Push(new TextToken(Current.Value));
                    break;

                case StringTokenType.Dash:
                    if (Next.Type == StringTokenType.WhiteSpace &&
                        (Previous.Type == StringTokenType.NewLine || position == 0))
                    {
                        if (TryOpenBodyWith(new ListItemOpenToken(Current.Value + Next.Value)))
                        {
                            position++;
                            break;
                        }
                    }

                    rootChildren.Push(new TextToken(Current.Value));
                    
                    break;

                case StringTokenType.Hash:
                    if (Next.Type != StringTokenType.WhiteSpace ||
                        (Previous.Type != StringTokenType.NewLine && position != 0))
                    {
                        rootChildren.Push(new TextToken(Current.Value));
                        break;
                    }

                    if (TryOpenBodyWith(new HeaderOpenToken(Current.Value + Next.Value)))
                    {
                        position++;
                        break;
                    }

                    rootChildren.Push(new TextToken(Current.Value));
                    break;
                case StringTokenType.SingleUnderscore:
                    if (TryOpenBodyWith(new ItalicOpenToken(Current.Value)))
                        break;

                    rootChildren.Push(new ItalicCloseToken(Current.Value));
                    children = UnitTokenEnds<ItalicOpenToken>()
                        .Select(token => token is StrongToken ? new TextToken(token.Value) : token).ToList();
                    temporaryStack.Pop();

                    if (children.IsOnlyDight())
                        rootChildren.Push(new TextToken(string.Join("", children.Select(child => child.Value))));
                    else
                        rootChildren.Push(new ItalicToken(children));
                    break;
                case StringTokenType.DoubleUnderscore:
                    if (TryOpenBodyWith(new StrongOpenToken(Current.Value)))
                        break;

                    rootChildren.Push(new StrongCloseToken(Current.Value));
                    children = UnitTokenEnds<StrongOpenToken>().ToList();
                    temporaryStack.Pop();

                    if (children.Count == 2)
                        rootChildren.Push(new TextToken(children[0].Value + children[1].Value));
                    else if (children.IsOnlyDight())
                        rootChildren.Push(new TextToken(string.Join("", children.Select(child => child.Value))));
                    else
                        rootChildren.Push(new StrongToken(children));

                    break;
                default:
                    throw new Exception("Unimplemented token");
            }

            position++;
        }

        if (temporaryStack.Any(pair => pair.token is HeaderOpenToken))
        {
            children = new List<BaseHtmlToken>() { new HeaderCloseToken("") };
            while (true)
            {
                var child = rootChildren.Pop();
                children.Add(child);
                if (child is HeaderOpenToken)
                    break;
            }

            children.Reverse();

            rootChildren.Push(new HeaderToken(children));
        }

        if (temporaryStack.Any(pair => pair.token is ListItemOpenToken))
        {
            children = new List<BaseHtmlToken>() { new ListItemCloseToken("") };
            while (true)
            {
                var child = rootChildren.Pop();
                children.Add(child);
                if (child is ListItemOpenToken)
                    break;
            }

            children.Reverse();

            rootChildren.Push(new HeaderToken(children));
        }

        
        return new RootToken(rootChildren.Reverse().TextifyTags().ToList());
    }

    private bool TryOpenBodyWith(BaseHtmlToken token)
    {
        //проврка на закрытие
        if (!temporaryStack.Any(pair => pair.token.GetType() == token.GetType()))
        {
            rootChildren.Push(token);
            temporaryStack.Push((position, token));
            return true;
        }

        //проверка на неверную вложенность
        if (temporaryStack.Peek().token.GetType() != token.GetType())
        {
            var last = temporaryStack.Pop().token;
            rootChildren.Push(new TextToken(Current.Value));
            while (last.GetType() != token.GetType())
                last = temporaryStack.Pop().token;
            return true;
        }


        if (Previous.Type == StringTokenType.WhiteSpace)
        {
            rootChildren.Push(new TextToken(token.Value));
            return true;
        }

        return false;
    }

    private IEnumerable<BaseHtmlToken> UnitTokenEnds<TOpenToken>() where TOpenToken : BaseHtmlToken
    {
        var list = new List<BaseHtmlToken>();
        while (true)
        {
            var child = rootChildren.Pop();
            list.Add(child);
            if (child is TOpenToken)
                break;
        }

        list.Reverse();
        yield return list.First();
        foreach (var node in list.Skip(1).Take(list.Count() - 2).TextifyTags())
            yield return node;
        yield return list.Last();
    }
}