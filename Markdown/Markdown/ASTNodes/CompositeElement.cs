namespace Markdown.ASTNodes
{
    public class CompositeElement : IElement
    {
        
        public IElement LeftChild { get; set; }
        public IElement Child { get; set; }

        public Token[] Tokens { get; }

        public CompositeElement(Token[] tokens)
        {
            Tokens = tokens;
        }
    }
}
