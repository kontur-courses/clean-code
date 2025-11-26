using Markdown.Core.Lexing;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing;

public interface IParser
{
    DocumentNode Parse(IEnumerable<Token> tokens);
}