using Markdown.Tags;
using System.Text;

namespace Markdown.Parsers
{
    public class EndLinkParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line)
        {
            if (line[i] != ']')
                return null;

            string address = null;
            if (line[i + 1] == '(' && line.IndexOf(')', i + 1) != -1)
            {
                var start = line.IndexOf('(', i);
                var finish = line.IndexOf(')', i);
                if (finish != start + 1)
                {
                    address = line.Substring(i + 2, finish - 1 - start);
                    line = line.Remove(start, finish - start + 1);
                }
            }

            return (address == null) ? null : new TagLink(address);
        }
    }
}
