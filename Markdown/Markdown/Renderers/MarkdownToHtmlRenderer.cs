using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;
using Markdown.Tokens.Tags;

namespace Markdown.Renderers;

public class MarkdownToHtmlRenderer()
{
    public string RenderWithConstraints(string markdown, bool allowBold)
    {
        var html = new StringBuilder();
        var tokenizer = new Tokenizer(allowBold);
        var tokens = tokenizer.Tokenize(markdown);

        for (var i = 0; i < tokens.Count; i++)
        {
            if (TryRenderMarkedList(tokens, i, html, out var newIndex))
            {
                i = newIndex;
                continue;
            }
            var token = tokens[i];
            if (token.Tag.Type == ETagType.Text)
            {
                html.Append(token.Value);
                continue;
            }

            var innerPartAllowBold = token.Tag.Type != ETagType.Italics;

            var innerHtml = RenderWithConstraints(token.Value, innerPartAllowBold);
            html.AppendHtml(token.Tag.HtmlTag, innerHtml);
        }

        return html.ToString();
    }

    private bool TryRenderMarkedList(List<Token> tokens, int index, StringBuilder html, out int newIndex)
    {
        if (!IsMarkedListItem(tokens[index]))
        {
            newIndex = index;
            return false;
        }

        var listTagType = tokens[index].Tag.Type;
        var lastItemIndex = index;

        html.Append("<ul>");
        
        for (var currentIndex = index; currentIndex < tokens.Count; )
        {
            if (!IsMarkedListItemOfType(tokens, currentIndex, listTagType))
                break;
            html.AppendHtml(tokens[currentIndex].Tag.HtmlTag, 
                RenderWithConstraints(tokens[currentIndex].Value, true));

            lastItemIndex = currentIndex;
            
            var lookAhead = currentIndex + 1;
            var newLines = 0;

            while (lookAhead < tokens.Count && tokens[lookAhead] is { Tag.Type: ETagType.Text, Value: "\n" })
            {
                newLines++;
                lookAhead++;
            }
            
            if (lookAhead < tokens.Count &&
                IsMarkedListItemOfType(tokens, lookAhead, listTagType) &&
                newLines < 2)
            {
                if (newLines > 0) html.Append('\n');
                currentIndex = lookAhead;
                continue;
            }

            break;
        }

        html.Append("</ul>");
        newIndex = lastItemIndex;

        return true;
    }

    private bool IsMarkedListItem(Token token)
    {
        return token.Tag.Type is ETagType.AsteriskMarkedList or ETagType.PlusMarkedList or ETagType.DashMarkedList;
    }

    private bool IsMarkedListItemOfType(List<Token> tokens, int index, ETagType tagType)
    {
        return index < tokens.Count &&
               tokens[index].Tag.Type == tagType;
    }
}