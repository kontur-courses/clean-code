using System;
using Markdown.IntermediateState;

namespace Markdown.Builders
{
    interface ILanguageBuilder
    {
        string BuildDocument(DocumentNode parsedDocument, Func<DocumentNode, string> unknownTagAction);
    }
}
