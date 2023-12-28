using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class LineParser
    {
        private readonly Dictionary<TagType, (int tokens, int symbols)> toSkip;
        public LineParser()
        {
            var headerTokenLength = Tokenizer.TypeToSymbols[TokenType.Header].Length;
            toSkip = new Dictionary<TagType, (int, int)>
            {
                [TagType.Header] = (1, headerTokenLength),
                [TagType.Line] = (0,0),
                [TagType.LastLine] = (0,0)
            };
        }

        public NestedTag Parse(Token[] tokens, string text, bool isLastLine)
        {
            var type = DefineLineType(tokens, isLastLine);

            var data = CorrectData((tokens, text), type);

            var inlineParser = new InlineParser();
            var tag = new NestedTag(type);

            tag.Tags = inlineParser.Parse(data.tokens, data.text);

            return tag;
        }

        private TagType DefineLineType(Token[] tokens, bool isLastLine)
        {
            if (tokens.Length != 0 && tokens[0].Type == TokenType.Header)
                return TagType.Header;
            else if (isLastLine)
                return TagType.LastLine;
            else
                return TagType.Line;
        }

        (Token[] tokens, string text) CorrectData((Token[] tokens, string text) data, TagType type)
        {
            var skip = toSkip[type];
            foreach (var token in data.tokens)
                token.Index -= skip.symbols;

   
            var newTokens = data.tokens.Length > 0 ? data.tokens[skip.tokens..] : data.tokens;
            var newText = data.text[skip.symbols..];

            return (newTokens, newText);
        }
    }
}

