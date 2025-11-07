using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;

namespace Markdown;

public class MarkdownParser
{
    private List<Token> tokens;
    private int position;
    public MarkdownParser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    public MarkdownDocumentNode ParseTokens()
    {
        throw new NotImplementedException();
    }

    private MarkdownNode ParseBlock()
    {
        throw new NotImplementedException();
    }

    private MarkdownNode ParseInline()
    {
        throw new NotImplementedException();
    }

    private HeaderNode ParseHeader()
    {
        throw new NotImplementedException();
    }
    
    private ImageNode ParseImage()
    {
        throw new NotImplementedException();
    }
    
    private BoldNode ParseStrong()
    {
        throw new NotImplementedException();
    }

    private ItalicNode ParseEmphasis()
    {
        throw new NotImplementedException();
    }
}
