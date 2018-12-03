using System.Linq;
using Markdown.Ecxeptions;

namespace Markdown
{
    public class MarkdownInterpreter
    {
        private readonly IElement rootNode;
        private readonly char[] badChars = new[] {'\t', ' ', '\n'};

        public MarkdownInterpreter(IElement rootNode)
        {
            this.rootNode = rootNode;
        }

        public string GetHtmlCode()
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
            var spaces = !badChars.Contains(value[0]) && !badChars.Contains(value[value.Length - 1]);
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
