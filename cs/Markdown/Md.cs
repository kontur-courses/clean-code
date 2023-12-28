using Markdown.Parsers;
using Markdown.Serializers;

namespace Markdown
{
    public class Md
    {
        private IMdParser mdParser;
        private ITokenSerializer serializer;

        public Md(IMdParser mdParser, ITokenSerializer serializer)
        {
            this.mdParser = mdParser;
            this.serializer = serializer;
        }

        public string Render(string rawMd)
        {
            var token = mdParser.Parse(rawMd);
            return serializer.Serialize(token);
        }
    }
}