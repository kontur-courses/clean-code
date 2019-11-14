using System;
using Markdown.IntermediateState;

namespace Markdown.Builders
{
    class HtmlBuilder : ILanguageBuilder
    {
        public string BuildDocument(DocumentNode parsedDocument, UnknownTagAction unknownTagAction = UnknownTagAction.Except)
        {
            throw new NotImplementedException();
        }
    }
}
