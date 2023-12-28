using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class LineParser
    {
        private readonly Dictionary<TagType, (int skipTags, int skipSymbols)> toSkip;
        public LineParser()
        {
            var headerTokenLength = Tokenizer.TypeToSymbols[TokenType.Header].Length;
            toSkip = new Dictionary<TagType, (int skipTags, int skipSymbols)>
            {
                [TagType.Header] = (1, headerTokenLength),
                [TagType.Line] = (0, 0),
                [TagType.LastLine] = (0, 0)
            };
        }

        public NestedTag Parse(Token[] tokens, string text, bool isLastLine)
        {
            var type = DefineLineType(tokens, isLastLine); 

            var inlineTags = tokens[toSkip[type].skipTags..];
            var inlineText = text[toSkip[type].skipSymbols..];

            var inlineParser = new InlineParser();
            var tag = new NestedTag(type);

            tag.Tags = inlineParser.Parse(inlineTags, inlineText);

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
    }
}

