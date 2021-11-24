namespace Markdown
{
    public class HyperTextElement
    {
        public HyperTextElement[] Children;
        public TextType Type;
        
        public HyperTextElement(TextType type, params HyperTextElement[] children)
        {
            Type = type;
            Children = children;
        }
    }

    public class HyperTextElement<T> : HyperTextElement
    {
        public T Value;
        
        public HyperTextElement(TextType type, T value) : base(type)
        {
            Value = value;
        }
        
        public HyperTextElement(TextType type, T value, params HyperTextElement[] children) : base(type, children)
        {
            Value = value;
        }
    }
}