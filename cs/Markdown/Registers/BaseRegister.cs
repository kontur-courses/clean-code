namespace Markdown.Registers
{
    abstract class BaseRegister
    {
        public abstract bool IsBlockRegister { get; }
        public abstract Token TryGetToken(string input, int startPos);
    }
}