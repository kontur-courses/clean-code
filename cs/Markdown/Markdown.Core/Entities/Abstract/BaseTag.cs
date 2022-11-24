namespace Markdown.Core.Entities.Abstract
{
    internal abstract class BaseTag
    {
        protected abstract int Priority { get; }
        public abstract Token TryGetToken(string input, int startPos);
    }
}
