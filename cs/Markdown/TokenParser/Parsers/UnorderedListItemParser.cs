using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class UnorderedListItemParser : IParser
{
    private readonly int nestedLevel;
    private readonly string bullet;

    public UnorderedListItemParser(int nestedLevel, string bullet)
    {
        this.nestedLevel = nestedLevel;
        this.bullet = bullet;
    }
    public Node? TryMatch(TokenList tokens)
    {
        if (!IsOpeningListItem(tokens, out var additionalConsumed))
        {
            return null;
        }

        var newTokens = new TokenList(
            new[] { new Token(TypeOfToken.StartOfParagraph) }
                .Concat(tokens.Skip(additionalConsumed + IndentationSize()))
        );
        additionalConsumed--;

        var sentenceParser = CreateSentenceParser();
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, sentenceParser);

        if (nodes.Count == 0)
        {
            return null;
        }
        
        consumed += additionalConsumed + IndentationSize();
        
        return new Node(TypeOfNode.ListItem, consumed, nodes);
    }
    
    private bool IsOpeningListItem(TokenList tokens, out int consumed)
    {
        consumed = default;
        
        if (!TryExtractListItemTokens(tokens, out var remainingTokens) || remainingTokens == null)
        {
            return false;
        }
        
        return MatchesOpening(remainingTokens, out consumed);
    }
    
    private bool TryExtractListItemTokens(TokenList tokens, out TokenList? remainingTokens)
    {
        remainingTokens = default;
        
        if (!tokens.PeekAt(2, Enumerable.Repeat(TypeOfToken.Whitespace, IndentationSize()).ToArray()))
        {
            return false;
        }
        
        remainingTokens = new TokenList(tokens.Take(2).Concat(tokens.Skip(IndentationSize() + 2)));
        return true;
    }
    
    private bool MatchesOpening(TokenList tokens, out int consumed)
    {
        consumed = default;
        var pattern = new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet, TypeOfToken.Whitespace };

        if (MatchesOpeningPattern(tokens, pattern))
        {
            consumed = pattern.Length;
            return true;
        }
        
        pattern = new[] { TypeOfToken.Newline, TypeOfToken.StartOfParagraph, TypeOfToken.Bullet, TypeOfToken.Whitespace };
        if (MatchesOpeningPattern(tokens, pattern))
        {
            consumed = pattern.Length;
            return true;
        }

        return false;
    }
    
    private bool MatchesOpeningPattern(TokenList tokens, params TypeOfToken[] pattern)
    {
        return tokens.Peek(pattern) && tokens[pattern.Length - 2].Value == bullet;
    }
    
    private SentenceParser CreateSentenceParser()
    {
        return  new SentenceParser(
            new UnorderedListParser(nestedLevel + 1), 
            new IntersectionParser(),
            new StrongParser(), 
            new EmphasisParser(), 
            new TextParser());
    }
    
    private int IndentationSize()
    {
        return nestedLevel * 4;
    }
}