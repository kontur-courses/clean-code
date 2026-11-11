using Markdown.Tokens;

namespace Markdown.Writers;

public interface IWriter
{
    public string Write(List<Token> tokens);
}