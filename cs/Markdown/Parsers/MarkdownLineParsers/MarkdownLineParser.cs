using System.Text;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public class MarkdownLineParser : IMarkdownLineParser
{
    public IEnumerable<IToken> ParseParagraphForTokens(string markdownLineParagraph)
    {
        var tokens = new List<IToken>();
        var previousTokenPositionInParagraph = -1;
        
        for (var i = 0; i < markdownLineParagraph.Length; i++)
        {
            if (tokens.Count > 0 && i < tokens[^1].Content.Length + previousTokenPositionInParagraph)
                continue;
            
            if (TagToken.TryCreateTagToken(markdownLineParagraph, i, tokens.Count, out var tagToken))
            {
                tokens.Add(tagToken!);
                previousTokenPositionInParagraph = i;
                continue;
            }
            
            var symbol = markdownLineParagraph[i];
            var commonTokenType = CommonToken.GetCommonTokenType(symbol);
            var tokenContent = symbol.ToString();

            if (commonTokenType == TokenType.Text)
                tokenContent += CompleteTextTokenContent(markdownLineParagraph, i + 1, tokens.Count, commonTokenType);

            var commonToken = CommonToken.CreateCommonToken(commonTokenType, tokenContent);
            tokens.Add(commonToken);
            previousTokenPositionInParagraph = i;
        }

        return tokens;
    }
    
    public IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownText)
    {
        var paragraphs = markdownText.Split(Environment.NewLine);

        foreach (var paragraph in paragraphs)
        {
            if (!string.IsNullOrWhiteSpace(paragraph))
                yield return paragraph;
        }
    }

    private static string CompleteTextTokenContent(string line, int positionInLine, int positionInTokens, TokenType tokenType)
    {
        var content = new StringBuilder();
        
        for (; positionInLine < line.Length; positionInLine++)
        {
            var currentTokenType = CommonToken.GetCommonTokenType(line[positionInLine]);
            
            if (tokenType != currentTokenType || 
                TagToken.TryCreateTagToken(line, positionInLine, positionInTokens, out _))
            {
                break;
            }

            content.Append(line[positionInLine]);
        }

        return content.ToString();
    }
}