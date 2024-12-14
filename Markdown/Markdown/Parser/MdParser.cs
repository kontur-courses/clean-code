using Markdown.ParseTree;
using Markdown.Token;

namespace Markdown.Parser;

public class MdParser(IParseTree<MdTokenType> parseTree) : IParser<MdTokenType, MdToken>
{
    public IParseTree<MdTokenType> Parse(IEnumerable<MdToken> tokens)
    {
        parseTree.OpenToken(MdTokenType.Line, ReadOnlyMemory<char>.Empty);
        foreach (var token in tokens)
        {
            if (token.Type == MdTokenType.Heading)
            {
                if (parseTree.CurrentToken is { TokenType: MdTokenType.Line, Empty: true })
                {
                    parseTree.OpenToken(token.Type, token.Text);
                }
                else
                {
                    parseTree.OpenToken(token.Type, token.Text);
                    parseTree.CloseCurrentToken(false);
                }
            }
            else if (token.Type == MdTokenType.Line)
            {
                while (parseTree.CurrentToken.TokenType != MdTokenType.Document)
                    parseTree.CloseCurrentToken(
                        parseTree.CurrentToken.TokenType is MdTokenType.Heading or MdTokenType.Line);
                parseTree.OpenToken(token.Type, token.Text);
            }
            else if (token.Behaviour == MdTokenBehaviour.Opening
                && parseTree.CurrentToken.TokenType != token.Type)
            {
                parseTree.OpenToken(token.Type, token.Text);
            }
            else if (token.Behaviour == MdTokenBehaviour.Closing
                     && parseTree.CurrentToken.TokenType == token.Type)
            {
                if (parseTree.CurrentToken.Empty)
                {
                    parseTree.CloseCurrentToken(false);
                    parseTree.OpenToken(token.Type, token.Text);
                    parseTree.CloseCurrentToken(false);
                }
                else
                    parseTree.CloseCurrentToken(true);
            }
            else if (token.Type == MdTokenType.PlainText)
            {
                parseTree.OpenToken(token.Type, token.Text);
                parseTree.CloseCurrentToken(true);
            }
            else if (token.Behaviour == MdTokenBehaviour.InsideAWord)
            {
                if (parseTree.CurrentToken.TokenType == token.Type)
                    parseTree.CloseCurrentToken(true);
                else
                    parseTree.OpenToken(token.Type, token.Text, true);
            }
            else
            {
                if (token.Behaviour == MdTokenBehaviour.Closing
                    && parseTree.CurrentToken.TokenType != MdTokenType.Document
                    && parseTree.CurrentToken.TokenType != MdTokenType.Line)
                    parseTree.CloseCurrentToken(false);
                parseTree.OpenToken(token.Type, token.Text);
                parseTree.CloseCurrentToken(false);
            }
        }
        
        while (parseTree.CurrentToken.TokenType != MdTokenType.Document)
            parseTree.CloseCurrentToken(
                parseTree.CurrentToken.TokenType is MdTokenType.Heading or MdTokenType.Line);

        return parseTree;
    }
}