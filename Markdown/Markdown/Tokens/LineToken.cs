namespace Markdown.Tokens;

public class LineToken : Token
{
    private static readonly IReadOnlyDictionary<Type, Type> containers = new Dictionary<Type, Type>
    {
        [typeof(UnorderedListItem)] = typeof(UnorderedListToken)
    };

    public LineToken(TokenType type = TokenType.Line, string opening = "", string ending = "") : base(opening, ending,
        type)
    {
        FirstPosition = 0;
    }

    public bool IsStackable { get; set; }

    public override bool CanStartsHere(string text, int index)
    {
        return base.CanStartsHere(text, index) && index == 0;
    }

    public static Type? GetContainerTypeOrDefault(LineToken item)
    {
        return containers.ContainsKey(item.GetType()) ? containers[item.GetType()] : null;
    }
}