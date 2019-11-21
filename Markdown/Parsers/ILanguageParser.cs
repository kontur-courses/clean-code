using Markdown.IntermediateState;

namespace Markdown.Parsers
{
    interface ILanguageParser
    {
        DocumentNode GetParsedDocument(string inputDocument);
    }
}
