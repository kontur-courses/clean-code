namespace Markdown.Core.Entities.Abstract
{
    public abstract class BaseTag
    {
        protected abstract int Priority { get; }
        public abstract Token TryGetToken(string input, int startPos);
    }
}
