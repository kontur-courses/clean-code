using System.Text;

namespace Markdown;

public class MdTokenizer
{
    private static Dictionary<string, ITag> dict;
    private readonly string text;

    public MdTokenizer(string text)
    {
        this.text = text;
        dict = new Dictionary<string, ITag>();
        dict.Add("_", new ItalicTag());
        dict.Add("# ", new HeaderTag());
        dict.Add("__", new BoldTag());
    }

    public IEnumerable<IToken> Tokenize()
    {
        var textBuilder = new StringBuilder();
        var tokens = new List<IToken>();
        for (var i = 0; i < text.Length;)
        {
            if (TryTokenizeFrom(i, out var token))
            {
                if (textBuilder.Length > 0)
                {
                    tokens.Add(new MdTextToken(textBuilder.ToString()));
                    textBuilder.Clear();
                }

                tokens.Add(token);
                i += token.Tag.MdTag.Length;
            }
            else if (text[i] == '\\')
            {
                if (textBuilder.Length > 0)
                {
                    tokens.Add(new MdTextToken(textBuilder.ToString()));
                    textBuilder.Clear();
                }
                
                tokens.Add(new MdEscapeToken());
                i++;
            }
            else if (text[i] == '\n')
            {
                if (textBuilder.Length > 0)
                {
                    tokens.Add(new MdTextToken(textBuilder.ToString()));
                    textBuilder.Clear();
                }
                
                tokens.Add(new MdNewlineToken());
                i++;
            }
            else textBuilder.Append(text[i++]);
        }

        if (textBuilder.Length != 0)
            tokens.Add(new MdTextToken(textBuilder.ToString()));




        for (var i = 0; i < tokens.Count - 1; i++)
        {
            if (tokens[i] is MdEscapeToken && tokens[i + 1] is MdTagToken token)
            {
                token.SetStatus(Status.Broken);
                tokens.RemoveAt(i);
            }
            else if (tokens[i] is MdEscapeToken && tokens[i + 1] is MdEscapeToken)
            {
                tokens.RemoveAt(i);
            }
            
        }
        
        var stack = new Stack<MdTagToken>();
        foreach (var tagToken in tokens.OfType<MdTagToken>())
        {
            if (tagToken.Status == Status.Broken)
                continue;
            if (stack.Count == 0)
                stack.Push((MdTagToken)tagToken);
            else
            {
                if (stack.Peek().Tag.MdTag == tagToken.Tag.MdTag)
                {
                    stack.Pop().SetStatus(Status.Opened);
                    tagToken.SetStatus(Status.Closed);
                }
                else
                {
                    stack.Push(tagToken);
                }
            }
        }

        foreach (var token in stack)
        {
            token.SetStatus(Status.Broken);
        }

        return tokens;
    }

    private bool TryTokenizeFrom(int position, out MdTagToken token)
    {
        token = null;
        var dasd = dict.Where(pairs => text[position..].StartsWith(pairs.Key)).ToArray();
        if (dasd.Any())
        {
            var tag = dasd.MaxBy(x => x.Value.MdTag.Length).Value;
            var context = new NeighboursContext(ElementAtOrNull(position - 1), ElementAtOrNull(position + tag.MdTag.Length));
            token = new MdTagToken(tag, context);
            return true;
        }
        
        return false;
    }

    private char? ElementAtOrNull(int position)
    {
        return position >= 0 && position < text.Length ? text[position] : null;
    }
}