namespace Markdown
{
    public abstract class ComplexTag : Tag
    {
        public abstract Tag InnerTag { get; }
    }
}
