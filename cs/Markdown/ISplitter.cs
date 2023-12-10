namespace Markdown;

public interface ISplitter
{ 
    IEnumerable<Token> SplitToTokens(string markdownText);
}