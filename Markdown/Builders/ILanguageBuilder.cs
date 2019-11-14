using Markdown.IntermediateState;

namespace Markdown.Builders
{
    interface ILanguageBuilder
    {
        string BuildDocument(DocumentNode parsedDocument, UnknownTagAction unknownTagAction = UnknownTagAction.Except);
    }
}
