namespace Markdown;

public enum NodeType
{
    OpenEmTag, 
    CloseEmTag,
    EmBody,
    OpenStrongTag,
    TextNode,
    WhitespaceNode, //TODO: delete maybe
    CloseStrongTag,
    StrongBody
}