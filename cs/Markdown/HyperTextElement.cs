namespace Markdown
{
    public class HyperTextElement
    {
        public HyperTextElement[] Children;
        public object Value;
        public string Type;
        
        public HyperTextElement(string type, object value, params HyperTextElement[] children)
        {
            Type = type;
            Children = children;
            Value = value;
        }

        public HyperTextElement(string type, params HyperTextElement[] children) : this(type, null, children){}
        public HyperTextElement(string type, object value) : this(type, value, new HyperTextElement[]{}){}
    }
}