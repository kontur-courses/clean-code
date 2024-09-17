using System.Text;

namespace Markdown.Parsers
{
    public class MdParser : IMdParser
    {
        private readonly ITokenizer tokenizer;

        public MdParser(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public MdDoc Parse(string input)
        {
            var blockParser = new BlockParser(tokenizer);
            return blockParser.Parse(input);
        }
    }
}