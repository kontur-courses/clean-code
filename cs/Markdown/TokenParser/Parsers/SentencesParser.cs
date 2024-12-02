using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class SentencesParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.Newline))
        {
            return null;
        }
        
        var sentenceParser = CreateSentenceParser();
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, sentenceParser);

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Paragraph, consumed, nodes);
    }
    
    private static SentenceParser CreateSentenceParser()
    {
        return new SentenceParser(
            new UnorderedListParser(), 
            new IntersectionParser(),
            new StrongParser(), 
            new EmphasisParser(), 
            new TextParser());
    }
}