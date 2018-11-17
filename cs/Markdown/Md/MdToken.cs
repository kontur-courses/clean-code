namespace Markdown.Md
{
    public class MdToken : IToken
    {
        public readonly string Value;

        public MdType Type { get; set; }

        public MdToken(MdType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}