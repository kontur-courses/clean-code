namespace Markdown
{
    internal enum TokenObjectType
    {
        Italic,
        Strong,
        Header
    }

    internal class ObjectToken : Token
    {
        public override TokenType Type => TokenType.Object;
        public TokenObjectType ObjectType { get; set; }
        public bool IsClose { get; set; }
    }
}