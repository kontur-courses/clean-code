namespace MarkdownProcessor.Tags;

public class TagsTree
{
    private readonly List<Tag> children = new();

    private readonly Dictionary<string, ITagMarkdownConfig> openingTokens;

    private readonly List<Tag> processedChildren = new();

    public TagsTree(Dictionary<string, ITagMarkdownConfig> openingTokens)
    {
        this.openingTokens = openingTokens;
    }

    public IEnumerable<Tag> ClosedTags => processedChildren
        .Concat(processedChildren.SelectMany(GetAllChildren))
        .Concat(children)
        .Concat(children.SelectMany(GetAllChildren))
        .Where(t => t.Closed);

    public void ProcessTokens(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            if (children.Count == 0 || children.Last().Closed)
            {
                var nullableTag = openingTokens.GetValueOrDefault(token.Value)?.CreateOrNull(token);
                if (nullableTag is not null) children.Add(nullableTag);
            }
            else
            {
                var nullableToken = children.Last().RunTokenDownOfTree(token);
                if (nullableToken is null) continue;

                var nullableTag = openingTokens.GetValueOrDefault(token.Value)?.CreateOrNull(token);
                if (nullableTag is not null) children.Last().RunTagDownOfTree(nullableTag);
            }

            if (token.Value == "\n")
            {
                processedChildren.AddRange(children);
                children.Clear();
            }
        }
    }

    private static IEnumerable<Tag> GetAllChildren(Tag tag)
    {
        return tag.Children.Concat(tag.Children.SelectMany(GetAllChildren));
    }
}