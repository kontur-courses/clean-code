using System.Collections.Generic;

namespace Markdown.IntermediateState
{
    public class DocumentNode
    {
        public int NestingLevel { get; }
        public TagType TypeTag { get; }
        public int BeginInnerPartInSource { get; }
        public int EndInnerPartInSource { get; }
        
        public IEnumerable<DocumentNode> InnerElements => innerElements;
        public IDictionary<string, string> Attributes => attributes;
        public string SourceDocument { get; }

        private readonly List<DocumentNode> innerElements;
        private Dictionary<string, string> attributes;

        public DocumentNode(TagType tagType, string sourceDocument, int beginInnerPartInSource, int endInnerPartInSource, int nestingLevel)
        {
            TypeTag = tagType;
            BeginInnerPartInSource = beginInnerPartInSource;
            EndInnerPartInSource = endInnerPartInSource;
            NestingLevel = nestingLevel;
            SourceDocument = sourceDocument;
            innerElements = new List<DocumentNode>();
            attributes = new Dictionary<string, string>();
        }

        public void AddElement(DocumentNode innerNode)
        {
            innerElements.Add(innerNode);
        }

        public void AddAttribute(string key, string value)
        {
            attributes[key] = value;
        }

        public bool ContainsInnerElements()
        {
            return innerElements.Count > 0;
        }
    }
}
