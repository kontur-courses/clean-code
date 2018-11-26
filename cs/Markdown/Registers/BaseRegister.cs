namespace Markdown.Registers
{
    abstract class BaseRegister
    {
        protected abstract int Priority { get; }
        public abstract bool IsBlockRegister { get; }
        public abstract Token TryGetToken(string input, int startPos);
    }
}