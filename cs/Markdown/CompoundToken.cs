using System.Text;

namespace Markdown;

internal class CompoundToken : Token
{
    public CompoundToken(string value, int sourceStart, TagSetting? setting) : base(value, sourceStart, setting)
    {
    }

    private readonly List<Token> tokens = new();

    internal void AddToken(Token token)
    {
        tokens.Add(token);
    }

    public override string Render()
    {
        var builder = new StringBuilder(Value);
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
            builder.Remove(token.SourceStart, token.Value.Length);
            builder.Insert(token.SourceStart, token.Render());
        }

        if (Setting != null)
            return Setting.Render(builder.ToString());

        return builder.ToString();
    }
}
