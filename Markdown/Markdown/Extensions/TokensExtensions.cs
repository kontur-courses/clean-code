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
                        else
                        {
                            var i1 = tokens.FindIndex(x => x == tagToken);
                            var i2 = tokens.FindIndex(x => x == lastOpened);
                            var range = tokens.GetRange(i2 + 1, i1 - i2 - 1);
                            if (range.Count == 0 || ((tagToken.Context.IsSurrounded() || lastOpened.Context.IsSurrounded()) && range.Any(x => x.Value.Contains(' '))))
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
                    }
                    else stack.Push(tagToken);
                }
            }
        }

        var tags = tokens.Where(x => x is MdTagToken tag && tag.Status != Status.Broken && tag.Tag.GetType() != typeof(HeaderTag)).Select(x => (MdTagToken)x).ToArray();
        for (var i = 0; i < tags.Length - 3; i++)
        {
            if (tags[i].Tag is BoldTag && tags[i + 1].Tag is ItalicTag && tags[i + 2].Tag is BoldTag &&
                tags[i + 3].Tag is ItalicTag)
            {
                foreach (var mdTagToken in tags[i..(i + 3)])
                {
                    mdTagToken.SetStatus(Status.Broken);
                }
            }
            else if (tags[i].Tag is ItalicTag && tags[i + 1].Tag is BoldTag && tags[i + 2].Tag is BoldTag &&
                     tags[i + 3].Tag is ItalicTag)
            {
                tags[i + 1].SetStatus(Status.Broken);
                tags[i + 2].SetStatus(Status.Broken);
            }
        }

        return tokens;
    }

    public static bool IsSurrounded(this (char, char) context)
    {
        return char.IsLetterOrDigit(context.Item1) && char.IsLetterOrDigit(context.Item2);
    }
}