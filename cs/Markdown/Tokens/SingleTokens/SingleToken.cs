namespace Markdown
{
    public abstract class SingleToken : Token
    {
        protected abstract string Value { get; }

        public override string GetValue()
        {
            return Value;
        }
    }
}
