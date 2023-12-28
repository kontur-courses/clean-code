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
            var lines = input.Split('\n');
            var mdDoc = new MdDoc();
            var lineParse = new LineParser();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var data = tokenizer.Tokenize(line);
                
                var tag = lineParse.Parse(data.tokens, data.newText, i == lines.Length - 1);

                mdDoc.Lines.Add(tag);
            }
            return mdDoc;
        }
    }
}