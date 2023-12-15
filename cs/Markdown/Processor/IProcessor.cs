using Markdown.Token;

namespace Markdown.Processor;

public interface IProcessor
{
    IList<IToken> ParseTags();
}