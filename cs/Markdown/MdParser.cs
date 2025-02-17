using System.Collections.Immutable;
using Markdown.Interfaces;

namespace Markdown;

public class MdParser : ILexer
{
    public IImmutableList<Token> Tokenize(string text)
    {
        var result = new List<Token>();
        var ptr = 0;
        var stack = new Stack<Token>();
        var pairTags = new List<PairToken>();
        while (ptr < text.Length)
        {
            if (IsStartLine(text, ptr) && ptr + 2 < text.Length && TokenType.Header.IsMatchMd(text.Substring(ptr, 2)))
            {
                result.Add(CreateTokenHeader(ptr, stack));
                ptr += 2;
            }

            else if (TokenType.NewLine.IsMatchMd(text[ptr]))
            {
                var token = CreateTokenNewLine(stack, ptr);
                result.Add(token);
                if (stack.TryPeek(out var startToken) && startToken.Type is TokenType.MarkerRange &&
                    (ptr + 1 >= text.Length || text[ptr + 1] != '*'))
                {
                    stack.Pop();
                    result.Add(new Token(" ", TokenType.MarkerRange, ptr) { IsTag = true });
                }

                ptr++;
            }

            else if (TokenType.BackSlash.IsMatchMd(text[ptr]))
            {
                if (!TryAddAsBackSlash(text, stack, ptr, result))
                {
                    stack.Push(new Token(@"\", TokenType.BackSlash, ptr) { IsTag = true });
                }

                ptr++;
            }

            else if (IsMarker(text, ptr))
            {
                if (!TryAddAsSingleTokenMarker(result, ptr, stack))
                {
                    CreatePairTokenMarker(stack, ptr, result);
                }

                ptr += 2;
            }

            else if (IsBold(text, ptr))
            {
                if (!TryAddAsSingleTokenUndercore(stack, ptr, text, TokenType.Bold, result))
                {
                    result.Add(CreatePairTokenUndercore(stack, ptr, text, pairTags, TokenType.Bold));
                }

                ptr += 2;
            }

            else if (IsItalic(text, ptr))
            {
                if (!TryAddAsSingleTokenUndercore(stack, ptr, text, TokenType.Italic, result))
                {
                    result.Add(CreatePairTokenUndercore(stack, ptr, text, pairTags, TokenType.Italic));
                }

                ptr++;
            }

            else
            {
                var tokenText = CreateSimpleToken(text[ptr].AsSimpleTokenType(), ptr, text);
                result.Add(tokenText);
                ptr += tokenText.Lenght;
            }
        }

        CheckPairToken(pairTags);
        return ImmutableList.CreateRange(result);
    }

    private static bool IsMarker(string text, int ptr)
    {
        return ptr + 1 < text.Length && TokenType.Marker.IsMatchMd(text.Substring(ptr, 2));
    }

    private static bool IsItalic(string text, int ptr) => TokenType.Italic.IsMatchMd(text[ptr]);

    private static bool IsBold(string text, int ptr) =>
        ptr + 1 < text.Length && TokenType.Bold.IsMatchMd(text.Substring(ptr, 2));


    private static bool TryAddAsBackSlash(string text, Stack<Token> stack, int ptr, List<Token> result)
    {
        if (OnLeftHaveTag(TokenType.BackSlash, stack, ptr))
        {
            stack.Pop();
            result.Add(new Token(@"\", TokenType.BackSlash, ptr));
            return true;
        }

        if (!BackSlashAsScreening(text, ptr))
        {
            result.Add(new Token(@"\", TokenType.BackSlash, ptr));
            return true;
        }

        return false;
    }

    private static bool BackSlashAsScreening(string text, int ptr)
    {
        return OnRightHave(TokenType.Italic, ptr + 1, text) ||
               OnRightHave(TokenType.BackSlash, ptr + 1, text) ||
               ptr + 1 < text.Length && text[ptr + 1] == '*';
    }

    private static bool TryAddAsSingleTokenMarker(List<Token> result, int ptr, Stack<Token> stack)
    {
        if (!OnLeftEmptyOrNewLine(result))
        {
            var marker = new Token("* ", TokenType.Marker, ptr);
            result.Add(marker);
            return true;
        }

        if (OnLeftHaveTag(TokenType.BackSlash, stack, ptr))
        {
            stack.Pop();
            result.Add(new Token("* ", TokenType.Marker, ptr));
            return true;
        }

        return false;
    }

    private static void CreatePairTokenMarker(Stack<Token> stack, int ptr, List<Token> result)
    {
        if (stack.TryPeek(out var token) && token.Type is TokenType.MarkerRange)
        {
            var marker = new Token("* ", TokenType.Marker, ptr) { IsTag = true };
            stack.Push(marker);
            result.Add(marker);
        }
        else
        {
            var marker = new Token("* ", TokenType.Marker, ptr) { IsTag = true };
            var markerRange = new Token(" ", TokenType.MarkerRange) { IsTag = true };
            result.Add(markerRange);
            result.Add(marker);
            stack.Push(markerRange);
            stack.Push(marker);
        }
    }

    private static bool OnLeftEmptyOrNewLine(List<Token> result)
    {
        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (result[i].Type is TokenType.WhiteSpace) continue;

            return result[i].Type is TokenType.NewLine;
        }

        return true;
    }

    private static Token CreateTokenHeader(int ptr, Stack<Token> stack)
    {
        var headerStart = new Token("# ", TokenType.Header, ptr) { IsTag = true };
        stack.Push(headerStart);
        return headerStart;
    }

    private static bool TryAddAsSingleTokenUndercore(Stack<Token> stack, int ptr,
        string text, TokenType type,
        List<Token> result)
    {
        var token = type.CreateTokenMd(ptr);
        if (OnLeftHaveTag(TokenType.BackSlash, stack, ptr))
        {
            stack.Pop();
            result.Add(token);
            return true;
        }

        if (OnRightHave(TokenType.Digit, ptr + token.Lenght, text) &&
            OnLeftHave(TokenType.Word, ptr - 1, text))
        {
            result.Add(token);
            return true;
        }

        if (OnRightHave(TokenType.Word, ptr + token.Lenght, text) &&
            OnLeftHave(TokenType.Digit, ptr - 1, text))

        {
            result.Add(token);
            return true;
        }

        return false;
    }

    private static bool IsStartLine(string text, int ptr) => ptr == 0 || text[ptr - 1] == '\n';

    private static void CheckPairToken(List<PairToken> possibleCorrectPair)
    {
        for (var i = 0; i < possibleCorrectPair.Count; i++)
        {
            possibleCorrectPair[i].Start.IsTag = true;
            possibleCorrectPair[i].End.IsTag = true;
            for (var j = Math.Max(i - 1, 0); j < possibleCorrectPair.Count; j++)
            {
                if (i == j || TagsCorrect(possibleCorrectPair[i], possibleCorrectPair[j])) continue;
                possibleCorrectPair[i].Start.IsTag = false;
                possibleCorrectPair[i].End.IsTag = false;

                break;
            }
        }
    }

    private static bool TagsCorrect(PairToken firstToken, PairToken secondToken)
    {
        return !firstToken.IntersectWith(secondToken) &&
               (!firstToken.Contain(secondToken) ||
                firstToken.Start.Type != TokenType.Bold);
    }

    private static bool OnLeftHaveTag(TokenType tokenType, Stack<Token> stack, int ptr)
    {
        return stack.TryPeek(out var token) && token.Type == tokenType && token.StartIndex + 1 == ptr;
    }


    private static Token CreateTokenNewLine(Stack<Token> stack, int ptr)
    {
        while (stack.TryPeek(out var token) && token.Type != TokenType.Header && token.Type != TokenType.Marker)
        {
            stack.Pop();
        }

        return new Token("\n", TokenType.NewLine, ptr)
            { IsTag = ParagraphStartWithTag(stack) };
    }

    private static Token CreatePairTokenUndercore(Stack<Token> stack, int ptr, string text,
        List<PairToken> possibleTags,
        TokenType type)
    {
        if (stack.TryPeek(out var token) && token.Type == type)
        {
            return CreateCloseToken(ptr, text, possibleTags, type, stack.Pop());
        }

        if (stack.TryPeek(out token) && token.Type != type)
        {
            var prevToken = stack.Pop();
            if (stack.TryPeek(out var tokenBold) && tokenBold.Type == type)
            {
                var closeToken = CreateCloseToken(ptr, text, possibleTags, type, stack.Pop());
                stack.Push(prevToken);
                return closeToken;
            }

            var newToken = type.CreateTokenMd(ptr);
            stack.Push(prevToken);
            if (!OnRightHave(TokenType.WhiteSpace, ptr + newToken.Lenght, text)) stack.Push(newToken);
            return newToken;
        }

        var bold = type.CreateTokenMd(ptr);
        if (!OnRightHave(TokenType.WhiteSpace, ptr + bold.Lenght, text)) stack.Push(bold);
        return bold;
    }

    private static bool OnRightHave(TokenType tokenType, int ptr, string text) =>
        ptr < text.Length && tokenType.IsMatchMd(text[ptr]);

    private static bool OnLeftHave(TokenType tokenType, int offset, string text) =>
        offset > 0 && tokenType.IsMatchMd(text[offset]);


    private static Token CreateCloseToken(int ptr, string text, List<PairToken> possibleTags, TokenType type,
        Token tokenOpen)
    {
        if (tokenOpen.EndIndex + 1 == ptr || char.IsWhiteSpace(text[ptr - 1])) return type.CreateTokenMd(ptr);

        var tokenClose = type.CreateTokenMd(ptr);
        possibleTags.Add(new PairToken(tokenOpen, tokenClose));
        return tokenClose;
    }

    private static bool ParagraphStartWithTag(Stack<Token> stack)
    {
        if (!stack.TryPeek(out var token) ||
            (token.Type != TokenType.Header && token.Type != TokenType.Marker)) return false;
        stack.Pop();
        return true;
    }

    private Token CreateSimpleToken(TokenType tokenType, int ptr, string text)
    {
        var start = ptr;
        var end = start;

        while (ptr < text.Length && tokenType.IsMatchMd(text[ptr])) end = ++ptr;


        return new Token(text.Substring(start, end - start), tokenType, start);
    }
}