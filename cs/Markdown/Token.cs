namespace Markdown;

internal class Token
{
    public readonly string Value;
    public readonly int SourceStart;
    protected readonly TagSetting? Setting;

    public Token(string token, int sourceStart, TagSetting? setting)
    {
        Value = token;
        SourceStart = sourceStart;
        this.Setting = setting;
    }

    public virtual string Render()
    {
        if (Setting != null)
            return Setting.Render(Value);

        return Value;
    }
}