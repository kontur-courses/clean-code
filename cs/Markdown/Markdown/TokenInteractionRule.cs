namespace Markdown;

public class TokenInteractionRule
{
    private Dictionary<Type, Type> shouldNotContain;
    private Dictionary<Type, string[]> shouldNotContainContent;

    public TokenInteractionRule()
    {
        shouldNotContain = new();
        shouldNotContainContent = new();
    }

    public TokenInteractionRule TagShouldNotBeContainedAnother<ParentTag, ChildTag>()
        where ParentTag : Tag, new()
        where ChildTag : Tag, new()
    {
        shouldNotContain.Add(typeof(ParentTag), typeof(ChildTag));
        return this;
    }

    public TokenInteractionRule TagShouldNotContainContent<T>(string[] content)
        where T : Tag, new()
    {
        shouldNotContainContent[typeof(T)] = content;
        return this;
    }

    public TokenInteractionRule TagShouldNotContainContent<T>(string content)
        where T : Tag, new() => TagShouldNotContainContent<T>(content.Select(c => c.ToString()).ToArray());

    public bool NodeMayBeAdded(TreeNode node)
    {
        return
            ShouldNotBeContainedTag(node)
            && ShouldNotContainContent(node);
    }

    private bool ShouldNotBeContainedTag(TreeNode node)
    {
        if (shouldNotContain.Count == 0)
            return true;
        var parent = node.Parent;
        var nodeTagType = node.Tag.GetType();
        while (parent != null)
        {
            var parentTagType = parent.Tag.GetType();
            if (shouldNotContain.ContainsKey(parentTagType)
                && shouldNotContain[parentTagType] == nodeTagType)
                return false;
            parent = parent.Parent;
        }

        return true;
    }

    private bool ShouldNotContainContent(TreeNode node)
    {
        if (shouldNotContainContent.Count == 0)
            return true;
        var tagType = node.Tag.GetType();
        return !(shouldNotContainContent.ContainsKey(tagType)
                 && shouldNotContainContent[tagType].Any(s => node.TaglessBody.Contains(s)));
    }
}