using System.Text;
using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown;

public class Tokenizer : ITokenizer
{
    private string line;
    private ITagCondition<TokenType> tagCondition;
    private ITokenBuilder tokenBuilder;
    private ITokenSetter<TokenType> tokenSeter;
    private ITokenTyper<TokenType> tokenTyper;

    public void Init(string line)
    {
        this.line = line;
        tagCondition = new TagCondition();
        tokenBuilder = new TokenBuilder(tagCondition);
        tokenTyper = new TokenTyper(line, tagCondition);
        tokenSeter = new TokenSetter(tokenBuilder);
    }

    public List<Token> TokenizeLine()
    {
        var tokens = new List<Token>();
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < line.Length; i++)
        {
            var type = tokenTyper.GetSymbolType(i);
            if (TagsIntersects(type))
            {
                CloseTags(tokens);
                tokenSeter.SetToken(tokens, TokenType.Text, ref i, line, stringBuilder);
            }
            else
            {
                tokenSeter.SetToken(tokens, type, ref i, line, stringBuilder);
            }
        }

        CloseTags(tokens);
        DeleteEmptyTags(tokens);
        return tokens;
    }


    private void CloseTags(List<Token> tokens)
    {
        CloseTag(tokens, TokenType.Italic);
        CloseTag(tokens, TokenType.Strong);
    }

    private void CloseTag(List<Token> tokens, TokenType type)
    {
        if (tagCondition.GetTagOpenStatus(type))
        {
            var token = tokens.First(x => x.Start == tagCondition.GetOpenIndex(type));
            var index = tokens.IndexOf(token);
            tokens[index] = new Text(token.Start, token.End, TokenType.Text, tagCondition.GetTag(type));
            tagCondition.CloseTag(type);
        }
    }

    private void DeleteEmptyTags(List<Token> tokens)
    {
        for (var i = 0; i < tokens.Count; i++)
            if (i + 1 != tokens.Count && tokens[i] is Tag && tokens[i + 1] is Tag)
                if (tokens[i + 1] is Tag tokenSecond && tokens[i] is Tag tokenFirst &&
                    tokenFirst.Type == tokenSecond.Type && tokenFirst.Status != tokenSecond.Status &&
                    tokenFirst.Status == TagStatus.Open)
                {
                    tokens[i] = TagToText(tokenFirst);
                    tokens[i + 1] = TagToText(tokenSecond);
                }
    }

    private Text TagToText(Tag tag)
    {
        return tokenBuilder.GetText(tag.Start, tagCondition.GetTag(tag.Type));
    }

    private bool TagsIntersects(TokenType type)
    {
        if (type != TokenType.Strong)
            return false;
        return tagCondition.GetTagOpenStatus(TokenType.Italic) && tagCondition.GetTagOpenStatus(TokenType.Strong);
    }
}