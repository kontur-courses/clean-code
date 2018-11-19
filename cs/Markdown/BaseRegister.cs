namespace Markdown
{
    public abstract class BaseRegister : IReadable
    {
        public abstract Token tryGetToken(ref string input, int startPos);
    }
}