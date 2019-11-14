using System.Collections.Generic;

namespace Markdown.IntermediateState
{
    class DocumentNode
    {
        public TagType TypeTag { get; }
        public int BeginInDocument { get; }
        public int EndInDocument { get; }
        public IEnumerable<DocumentNode> InnerElements => innerElements;

        private List<DocumentNode> innerElements;

        public DocumentNode(TagType tagType, int beginInDocument, int endInDocument)
        {
            TypeTag = tagType;
            BeginInDocument = beginInDocument;
            EndInDocument = endInDocument;
        }

        public void AddElement(DocumentNode innerNode)
        {
            innerElements.Add(innerNode);
        }
    }
}
