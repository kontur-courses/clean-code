using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Processor;

public class Processor
{
    private ISyntax syntax;
    private string text;

    public Processor(ISyntax syntax, string text)
    {
        this.syntax = syntax;
        this.text = text;
    }

    public IList<IToken> ParseTags(string text)
    {
        var tags = FindAllTags(text);
        tags = RemoveEscapedTags(tags);
        tags = RemoveNonPairTags(tags);
        return GetValidTags(tags);
    }

    private IList<IToken> FindAllTags(string text)
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