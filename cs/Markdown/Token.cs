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
        this.setting = setting;
    }

    public virtual string Render()
    {
        if (setting != null)
            return setting.Render(Value);

        return Value;
    }
}