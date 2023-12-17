using Markdown.Contracts;
using Markdown.Tags;
using Markdown.Tokens;
using System.Text;

namespace Markdown;

public class MD
{
    private readonly Dictionary<string, Func<ITag>> tagFactory = new();
    private readonly Tokenizer tokenizer;

    public MD()
    {
        tokenizer = new Tokenizer(this);
    }

    public string Render(string text)
    {
        var tokens = tokenizer.CollectTokens(text);
        return CombineString(tokens);
    }

    public void AddFactoryFor(string mark, Func<ITag> creationMethod)
    {
        if (mark == null)
            throw new NullReferenceException("Tag mark can't be null");

        tagFactory.Add(mark, creationMethod);
    }

    public ITag? GetInstanceViaMark(string mark)
    {
        foreach (var tagMark in tagFactory.Keys)
            if (mark.StartsWith(tagMark))
                return tagFactory[tagMark]();

        return null;
    }

    private static string CombineString(IList<Token> tokens)
    {
        var builder = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Tag == null)
            {
                builder.Append(token.Text);
                continue;
            }

            var tokenTag = token.Tag!;

            switch (tokenTag.Status)
            {
                case TagStatus.Broken:
                    builder.Append(tokenTag.Info.GlobalMark);
                    break;
                case TagStatus.Open:
                    builder.Append(tokenTag.Info.OpenMark);
                    break;
                default:
                    builder.Append(tokenTag.Info.CloseMark);
                    break;
            }
        }

        return builder.ToString();
    }
}