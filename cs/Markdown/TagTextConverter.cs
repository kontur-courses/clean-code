namespace Markdown;

public class TagTextConverter
{
    private string markdownText;

    public string ToHTMLCode(string markdownText)
    {
        this.markdownText = markdownText;

        return ReplaceMdWithHTML();
    }

    private string ReplaceMdWithHTML()
    {
        throw new NotImplementedException();
    }

    private List<((int openIdx, int closeIdx) tagPair, string tag)> GetPairTagsIndexes()
    {
        throw new NotImplementedException();
    }

    private List<int> GetSharpIndexes()
    {
        throw new NotImplementedException();
    }

    private List<int> GetShieldingIndexes()
    {
        throw new NotImplementedException();
    }

    private List<((int openIdx, int closeIdx) tagPair, string tag)> RemoveIntersectStrongAndEmTags(
        List<((int openIdx, int closeIdx) tagPair, string tag)> pairTagsIndexes)
    {
        throw new NotImplementedException();
    }

    private List<((int openIdx, int closeIdx) tagPair, string tag)> RemoveStrongInsideEmTags(
        List<((int openIdx, int closeIdx) tagPair, string tag)> pairTagsIndexes)
    {
        throw new NotImplementedException();
    }
}