using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Processor;

public class Processor
{
    private string text;

    public Processor(string text)
    {
        this.text = text;
    }

    public IList<IToken> ParseTags()
    {
        var tags = FindAllTags();
        tags = RemoveEscapedTags(tags);
        tags = RemoveNonPairTags(tags);
        return GetValidTags(tags);
    }

    private IList<IToken> FindAllTags()
    {
        
    }

    private IList<IToken> RemoveEscapedTags(IList<IToken> tags)
    {
        
    }

    private IList<IToken> RemoveNonPairTags(IList<IToken> tags)
    {
        
    }

    private IList<IToken> GetValidTags(IList<IToken> tags)
    {
        
    }
}