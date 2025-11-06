using Markdown.Html;
using Markdown.TokenParser;

namespace Markdown;

public class MarkdownProcessor
{
    private readonly IParagraphCreator _paragraphCreator;
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;
    private readonly IHtmlCreator _htmlCreator;
    
    public MarkdownProcessor()
    {
        _paragraphCreator = new ParagraphCreator();
        _tokenizer = new Tokenizer();
        _parser = new Parser();
        _htmlCreator = new HtmlCreator();
    }

    public string ConvertToHtml(string markdownString)
    {
        throw new NotImplementedException();
    }
}