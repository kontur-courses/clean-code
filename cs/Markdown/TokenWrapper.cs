namespace Markdown;

internal class TokenWrapper
{
    public WrapperSettingsProvider Settings { get; private set; }

    public TokenWrapper(WrapperSettingsProvider settings)
    {
        Settings = settings;
    }

    public string WrapToken(Token token)
    {
        throw new NotImplementedException();
    }
}
