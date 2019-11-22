using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public abstract class SyntaxNode
    {
        public List<SyntaxNode> ChildNode { get; }

        protected SyntaxNode(List<SyntaxNode> childNode)
        {
            ChildNode = childNode;
        }

        public string ConvertTo<T>() where T : ILanguage, new()
        {
            return ConvertTo(new T().Tags);
        }

        public abstract string ConvertTo(LanguageTagDict languageTagDict);
    }
}