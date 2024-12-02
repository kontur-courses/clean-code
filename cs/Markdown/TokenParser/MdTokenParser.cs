using System;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;
using Markdown.TokenParser.Parsers;

namespace Markdown.TokenParser;

public class MdTokenParser : IMdTokenParser
{
    public Node Parse(TokenList tokens)
    {
        var body = new BodyParser().TryMatch(tokens);

        if (body == null)
        {
            throw new Exception("Body is null");
        }
        
        if (tokens.Count() != body.Consumed)
        {
            throw new Exception($"Syntax error: {tokens[body.Consumed]}");
        }

        return body;
    }
}