namespace Markdown;

internal class TokenWrapper
{
    public WrapperSettingsProvider Settings { get; private set; }

    public TokenWrapper(WrapperSettingsProvider settings)
    {
        Settings = settings;
    }

    public WrappedToken WrapToken(Token token)
    {
        var setting = Settings.FirstOrDefault(x => token.Text.StartsWith(x.Key!)).Value;

        return new(token, setting);
    }
}
