namespace Markdown;

public class MarkdownTokenizer
{
    private int position;
    private string markdownText;
    public MarkdownTokenizer(string markdownText)
    {
        this.markdownText = markdownText;
    }
    public List<Token> Tokenize()
    {
        throw new NotImplementedException();
    }

    private Token TokenizeHeader()
    {
        throw new NotImplementedException();
    }

    private Token TokenizeUnderscore()
    {
        throw new NotImplementedException();
    }

    private Token TokenizeImage()
    {
        throw new NotImplementedException();
    }

    private Token TokenizeText()
    {
        throw new NotImplementedException();
    }
}