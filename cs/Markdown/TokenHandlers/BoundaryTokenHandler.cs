using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public abstract class BoundaryTokenHandler : ITokenHandler
{
    protected abstract string Delimiter { get; }
    protected abstract TokenType TokenType { get; }

    public abstract bool CanHandle(char current, char next, MarkdownParseContext context);

    public void Handle(MarkdownParseContext context)
    {
        if (IsValidBoundary(context))
        {
            HandleTokenBoundary(context);
        }
        else
        {
            context.Buffer.Append(Delimiter);
            context.CurrentIndex += Delimiter.Length;
        }
    }

    private bool IsValidBoundary(MarkdownParseContext context)
    {
        var index = context.CurrentIndex;
        var text = context.MarkdownText;
        var nextDelimiter = TokenType == TokenType.Strong ? "_" : "__";
        if (context.IntersectedIndexes.Contains(index))
            return false;

        if (context.Stack.Count > 0)
        {
            if (context.Stack.Peek().Type == TokenType.Emphasis && TokenType == TokenType.Strong)
            {
                return false;
            }

            if (context.Stack.Count > 2)
            {
                return false;
            }

            if (context.Buffer.Length == 0)
                return false;

            if (index == 0 || index == text.Length - 1)
                return true;

            var spaceIndex = text.IndexOf(' ', index + Delimiter.Length);
            if (spaceIndex == -1)
                return true;

            return !char.IsLetterOrDigit(text[index - 1]) ||
                   !char.IsLetterOrDigit(text[index + 1]);
        }

        var paragraphEndIndex = text.IndexOfAny(['\n', '\r'], index);
        if (paragraphEndIndex == -1)
        {
            paragraphEndIndex = text.Length;
        }

        var closingIndex = FindSingleDelimiter(text, 
            index + Delimiter.Length, paragraphEndIndex, Delimiter);
        var anotherOpenIndex = FindSingleDelimiter(text, 
            index + Delimiter.Length, paragraphEndIndex, nextDelimiter);
        var anotherClosingIndex = FindSingleDelimiter(text, 
            anotherOpenIndex + nextDelimiter.Length, paragraphEndIndex, nextDelimiter);

        if (anotherOpenIndex < closingIndex && anotherClosingIndex > closingIndex)
        {
            context.IntersectedIndexes.Add(index);
            context.IntersectedIndexes.Add(closingIndex);
            context.IntersectedIndexes.Add(anotherOpenIndex);
            context.IntersectedIndexes.Add(anotherClosingIndex);
            return false;
        }

        if (closingIndex == -1)
            return false;

        var isInsideWord = (index > 0 && char.IsLetterOrDigit(text[index - 1])) ||
                           (closingIndex + Delimiter.Length < paragraphEndIndex &&
                            char.IsLetterOrDigit(text[closingIndex + Delimiter.Length]));
        if (isInsideWord)
        {
            if (index > 0 && 
                    (char.IsDigit(text[index - 1]) || 
                    char.IsDigit(text[index + 1])) && 
                    closingIndex + Delimiter.Length < paragraphEndIndex && 
                    (char.IsDigit(text[closingIndex - 1]) || 
                    char.IsDigit(text[closingIndex + Delimiter.Length])))
                return false;

            var spaceIndex = text.IndexOf(' ', index + Delimiter.Length);

            return spaceIndex == -1 || closingIndex < spaceIndex;
        }

        if (closingIndex - index <= Delimiter.Length)
            return false;

        return index + 1 != closingIndex;
    }

    private void HandleTokenBoundary(MarkdownParseContext context)
    {
        MarkdownParser.AddToken(context, TokenType.Text);

        if (context.Stack.Count > 0 && context.Stack.Peek().Type == TokenType)
        {
            var completedToken = context.Stack.Pop();

            completedToken.Content = completedToken.Children.Count > 0 ? 
                string.Empty : completedToken.Content;
            context.Buffer.Clear();

            if (context.Stack.Count > 0)
                context.Stack.Peek().Children.Add(completedToken);
            else
                context.Tokens.Add(completedToken);
        }
        else
        {
            var newToken = new BaseToken(TokenType);
            context.Stack.Push(newToken);
        }

        context.CurrentIndex += Delimiter.Length;
    }

    private static int FindSingleDelimiter(string text, int startIndex, int paragraphEndIndex, string delimiter)
    {
        var index = text.IndexOf(delimiter, startIndex, StringComparison.Ordinal);

        while (index != -1 && index < paragraphEndIndex)
        {
            if (index > 0 && text[index - 1] == '_')
            {
                index = text.IndexOf(delimiter, index + 1, StringComparison.Ordinal);
                continue;
            }

            if (index + delimiter.Length < text.Length && text[index + delimiter.Length] == '_')
            {
                index = text.IndexOf(delimiter, index + 2, StringComparison.Ordinal);
                continue;
            }
            return index;
        }
        return -1;
    }
}