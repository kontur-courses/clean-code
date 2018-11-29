namespace Markdown.Registers
{
    internal abstract class BaseRegister
    {
        protected abstract int Priority { get; }
        public abstract Token TryGetToken(string input, int startPos);
    }
}