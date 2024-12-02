using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class ParagraphParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        return MatchesFirst.MatchFirst(tokens, new NewlineParser(), new HashSentences(), new SentencesParser());
    }
}