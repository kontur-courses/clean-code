using Markdown.Element;

namespace Markdown.Token
{
    public class Token
    {
        public string Content;
        public IElement Element;

        public Token(string content, IElement element)
        {
            Content = content;
            Element = element;
        }
    }
}
