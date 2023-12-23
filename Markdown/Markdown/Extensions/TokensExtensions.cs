namespace Markdown.Extensions;

public static class TokensExtensions
{
    public static List<IToken> HandleTokens(this List<IToken> tokens)
    {
        var stack = new Stack<MdTagToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (i < tokens.Count - 1 && token is MdEscapeToken escapeToken)
                escapeToken.Escape(tokens[i + 1]);
            if (token is MdNewlineToken or MdEndOfTextToken)
                while (stack.Count > 0)
                {
                    var ex = stack.Pop();
                    if (ex.Tag is HeaderTag)
                    {
                        ex.SetStatus(Status.Opened);
                        var closed = new MdTagToken(new HeaderTag());
                        closed.SetStatus(Status.Closed);
                        tokens.Insert(i, closed);
                    }
                    else
                        ex.SetStatus(Status.Broken);   
                }
            else if (token is MdTagToken tagToken)
            {
                if (tagToken.Status == Status.Broken)
                    continue;
                if (stack.Count == 0)
                    if (tagToken.Context.Item2 == ' ' || char.IsDigit(tagToken.Context.Item1) || char.IsDigit(tagToken.Context.Item2))
                        tagToken.SetStatus(Status.Broken);
                    else
                        stack.Push(tagToken);
                else if (stack.Count > 0)
                {
                    var lastOpened = stack.Peek();
                    if (lastOpened.Tag == tagToken.Tag)
                    {
                        if (tagToken.Context.Item1 == ' ' || char.IsDigit(tagToken.Context.Item1) || char.IsDigit(tagToken.Context.Item2))
                            tagToken.SetStatus(Status.Broken);
                        else if (tokens.FindIndex(x => x == tagToken) - tokens.FindIndex(x => x == lastOpened) == 1)
                        {
                            stack.Pop().SetStatus(Status.Broken);
                            tagToken.SetStatus(Status.Broken);
                        }
                        else 
                        {
                            stack.Pop().SetStatus(Status.Opened);
                            tagToken.SetStatus(Status.Closed);
                        }
                    }
                    else if (lastOpened.Tag is ItalicTag && tagToken.Tag is BoldTag)
                    {
                        tagToken.SetStatus(Status.Broken);
                    }
                    else stack.Push(tagToken);
                }
            }
        }

        return tokens;
    }
}