using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    public readonly struct ParagraphData
    {
        public readonly Token[] Tokens;

        public ParagraphData(Token[] tokens)
        {
            Tokens = tokens;
        }

        public static ParagraphData FromTokens(IEnumerable<Token> tokens) => new ParagraphData(tokens.ToArray());
        public static ParagraphData FromWorker(TokenizationWorker worker)
        {
            worker.ParseTokens();
            return FromTokens(worker.ParsedTokens);
        }
    }
}