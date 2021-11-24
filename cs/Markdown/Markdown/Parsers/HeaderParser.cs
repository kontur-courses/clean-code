using System.Text;
using Markdown.Tags;

namespace Markdown.Parsers
{
    public class HeaderParser : IParser
    {
        public IToken TryGetToken()
        {
            return new TagHeader();
        }
        /*
        public IToken TryGetToken()
        {
            throw new System.NotImplementedException();
        }
        */
    }
}
