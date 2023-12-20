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

    public IEnumerable<MdToken> Tokenize()
    {
        var textBuilder = new StringBuilder();
        var tokens = new List<MdToken>();
        for (var i = 0; i < text.Length;)
        {
            if (TryTokenizeFrom(i, out var token))
            {
                if (textBuilder.Length > 0)
                {
                    tokens.Add(new MdToken(textBuilder.ToString()));
                    textBuilder.Clear();
                }

                tokens.Add(token);
                i += token.Tag.MdTag.Length;
            }
            else textBuilder.Append(text[i++]);
        }

        if (textBuilder.Length != 0)
            tokens.Add(new MdToken(textBuilder.ToString()));

        return tokens;
    }

    private bool TryTokenizeFrom(int position, out MdToken token)
    {
        token = null;
        var dasd = dict.Where(pairs => text[position..].StartsWith(pairs.Key)).ToArray();
        if (dasd.Any())
        {
            token = new MdToken(dasd.MaxBy(x => x.Value.MdTag.Length).Value);
            return true;
        }
        
        return false;
    }
}