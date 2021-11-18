using System.Text;

namespace Markdown;

internal class CompoundToken : Token
{
    public CompoundToken(string value, int sourceStart, TagSetting? setting) : base(value, sourceStart, setting)
    {
    }

    public List<Token> Tokens { get; private set; } = new();

    internal void AddToken(Token token)
    {
        Tokens.Add(token);
    }

    public override string Render()
    {
        var builder = new StringBuilder(Value);
        for (var i = Tokens.Count - 1; i >= 0; i--)
        {
            var token = Tokens[i];
            builder.Remove(token.SourceStart, token.Value.Length);
            builder.Insert(token.SourceStart, token.Render());
        }

        if (setting != null)
            return setting.Render(builder.ToString());

        return builder.ToString();
    }
}
