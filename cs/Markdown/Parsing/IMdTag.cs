using Markdown.Reading;

namespace Markdown.Parsing;

public interface IMdTag
{
    string Name { get; }
    bool CanBeParsed { get; }
    
    bool NeedEscape(Token token);
    string ClearText(string text);
    List<PatternTree> CheckFirstCharMatch(Token token);
}