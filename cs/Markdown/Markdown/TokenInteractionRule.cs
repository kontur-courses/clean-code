namespace Markdown;

public class TokenInteractionRule
{
    private Dictionary<Tag, Tag> shouldNotContain;
    private Dictionary<Tag, string[]> shouldNotContainContent;

    public TokenInteractionRule()
    {
        shouldNotContain = new();
        shouldNotContainContent = new();
    }

    public TokenInteractionRule TagShouldNotBeContainedAnother<ParentTag, ChildTag>()
        where ParentTag : Tag, new()
        where ChildTag : Tag, new()
    {
        shouldNotContain.Add(new ParentTag(), new ChildTag());
        return this;
    }

    public TokenInteractionRule TagShouldNotContainContent<T>(string[] content)
        where T : Tag, new()
    {
        shouldNotContainContent[new T()] = content;
        return this;
    }

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
        while (parent != null)
        {
            if (shouldNotContain.ContainsKey(parent.Tag)
                && shouldNotContain[parent.Tag] == node.Tag)
                return false;
            parent = parent.Parent;
        }

        return true;
    }

    private bool ShouldNotContainContent(TreeNode node)
    {
        if (shouldNotContainContent.Count == 0)
            return true;
        var tag = node.Tag;
        return !(shouldNotContainContent.ContainsKey(tag)
                 && shouldNotContainContent[tag].Any(s => node.TaglessBody.Contains(s)));
    }
}