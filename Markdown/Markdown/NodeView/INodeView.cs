namespace Markdown.NodeView;

public interface INodeView<TTokenType>
{
    public ReadOnlyMemory<char> Text { get; set; }
    public TTokenType Type { get; set; }
    public bool InsideWord { get; set; }
    public List<INodeView<TTokenType>> Children { get; set; }
    public INodeView<TTokenType>? Parent { get; set; }
}