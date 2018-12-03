using Markdown.Ecxeptions;

namespace Markdown
{
    public class MarkdownInterpreter : IInterpreter
    {
        private readonly IElement rootNode;

        public MarkdownInterpreter(IElement rootNode)
        {
            this.rootNode = rootNode;
        }

        public string Interpret()
        {
            return Execute(rootNode);
        }

        private string Execute(IElement node)
        {
            if (node == null)
                return "";
            switch (node)
            {
                case CompositeElement compositeElement:
                {
                    var mainChild = Execute(compositeElement.Child);
                    var leftChild = Execute(compositeElement.LeftChild);
                    return leftChild + mainChild;
                }
                case IStyleElement styleElement:
                {
                    var childValue = Execute(node.Child);
                    return IsCorrectToken(childValue) ? styleElement.TransformToHtml(childValue)
                        : styleElement.GetRawValue(childValue);
                }              
                case SimpleElement element:
                    return element.Value;
            }
            
            throw new UnknownElementException(node);
        }


        private bool IsCorrectToken(string value)
        {
            var spaces = !char.IsWhiteSpace(value[0]) && !char.IsWhiteSpace(value[value.Length - 1]);
            var allIsDigits = true;
            foreach (var sym in value)
            {
                if (char.IsDigit(sym)) continue;
                allIsDigits = false;
                break;
            }

            return spaces && !allIsDigits;
        }


    }
}
