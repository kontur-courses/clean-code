namespace Markdown;

public class Tokenizer
{
    private int position;
    private string text = string.Empty;

    private char Current => position < text.Length ? text[position] : '\0';
    private char Next => position + 1 < text.Length ? text[position + 1] : '\0';
    private char Previous => position - 1 >= 0 ? text[position - 1] : '\0';
    private bool Eof => position >= text.Length;

    private void Advance(int count = 1) => position += count;

    public List<Token> Tokenize(string markdownText)
    {
        text = markdownText;
        position = 0;
        var tokens = new List<Token>();
        var italicOpen = false;
        var boldOpen = false;

        while (!Eof)
        {
            switch (Current)
            {
                case '\\':
                {
                    ParseEscape(tokens);
                    continue;
                }
                case '#':
                {
                    ParseHeading(tokens);
                    continue;
                }
                case '\n':
                case '\r':
                {
                    ParseNewLine(tokens);
                    continue;
                }
                case '_':
                {
                    ParseUnderscore(ref italicOpen, ref boldOpen, tokens);
                    continue;
                }

                case '-':
                case '*':
                case '+': 
                {
                    if (IsListItemStart())
                    {
                        ParseListItem(tokens);
                        continue;
                    }
                    break;
                }
            }

            ParseText(tokens);
        }

        tokens.Add(Token.EndOfFile());
        return ConvertUnmatchedTokensToText(tokens);
    }
    
    private bool IsListItemStart()
    {
        return (position == 0 || Previous == '\n' || Previous == '\r') && Next == ' ';
    }

    private void ParseListItem(List<Token> tokens)
    {
        Advance(2); 

        tokens.Add(Token.ListItem());

        var contentStart = position;
        while (!Eof && Current != '\n' && Current != '\r')
            Advance();

        if (position > contentStart)
        {
            var content = text[contentStart..position];
            var contentTokens = TokenizeListItemContent(content);
            tokens.AddRange(contentTokens);
        }

        if (!Eof && Current is '\n' or '\r')
        {
            ParseNewLine(tokens);
        }
    }

    private static List<Token> TokenizeListItemContent(string content)
    {
        var tempTokenizer = new Tokenizer();
        var tokens = tempTokenizer.Tokenize(content);
        
        return tokens.Where(t => t.Type != TokenType.EndOfFile).ToList();
    }
    
    private void ParseUnderscore(ref bool italicOpen, ref bool boldOpen, List<Token> tokens)
    {
        var start = position;
        var count = 1;

        while (Next == '_')
        {
            count++;
            Advance();
        }

        if (HandleUnderscoreEdgeCases(italicOpen, tokens, ref count)) 
            return;

        var kind = AnalyzeUnderscore(text, start, count, ref italicOpen, ref boldOpen);
        AddToken(tokens, kind, count);

        Advance();
    }

    private bool HandleUnderscoreEdgeCases(bool italicOpen, List<Token> tokens, ref int count)
    {
        if (Next != ' ' || italicOpen || count != 1) return false;
        tokens.Add(Token.Text("_"));
        Advance();
        return true;
    }

    private static void AddToken(List<Token> tokens, EmphasisType kind, int count)
    {
        var toAdd = kind switch
        {
            EmphasisType.ItalicStart => new[] { Token.ItalicStart() },
            EmphasisType.ItalicEnd => new[] { Token.ItalicEnd() },
            EmphasisType.BoldStart => new[] { Token.BoldStart() },
            EmphasisType.BoldEnd => new[] { Token.BoldEnd() },
            EmphasisType.BoldItalicStart => new[] { Token.BoldStart(), Token.ItalicStart() },
            EmphasisType.BoldItalicEnd => new[] { Token.ItalicEnd(), Token.BoldEnd() },
            _ => new[] { Token.Text(new string('_', count)) }
        };

        tokens.AddRange(toAdd);
    }

    private void ParseText(List<Token> tokens)
    {
        var textStart = position;
        while (!Eof && Current != '_' && Current != '\n' && Current != '\r' && Current != '#' && Current != '\\')
            Advance();

        if (position > textStart)
            tokens.Add(Token.Text(text[textStart..position]));
    }

    private void ParseNewLine(List<Token> tokens)
    {
        while (Current is '\n' or '\r')
            Advance();
        tokens.Add(Token.NewLine());
    }

    private void ParseEscape(List<Token> tokens)
    {
        var backslashCount = 0;

        while (!Eof && text[position] == '\\')
        {
            backslashCount++;
            Advance();
        }

        if (Eof)
        {
            tokens.Add(Token.Text(new string('\\', backslashCount)));
            return;
        }

        var next = Current;

        var escapable =
            next is '\\' or '_' or '#' or '*' or '+' or '-' or '`';

        if (backslashCount % 2 == 1 && !escapable)
        {
            tokens.Add(Token.Text(new string('\\', backslashCount) + next));
            Advance();
            return;
        }

        var pairs = backslashCount / 2;
        if (pairs > 0)
            tokens.Add(Token.Text(new string('\\', pairs)));

        if (backslashCount % 2 != 1) 
            return;
        
        var escaped = Current;

        if (escaped == '#')
        {
            var start = position;
            while (!Eof && Current == '#')
                Advance();
            tokens.Add(Token.Text(text[start..position]));
        }
        else
        {
            tokens.Add(Token.Text(escaped.ToString()));
            Advance();
        }
    }

    private void ParseHeading(List<Token> tokens)
    {
        var start = position;
        var hashCount = 0;
    
        while (Current == '#' && hashCount < 6)
        {
            hashCount++;
            Advance();
        }
    
        var headingText = text[start..position];
    
        if (hashCount == 6 && position < text.Length && text[position] == '#')
        {
            var extraHashesStart = position;
            while (Current == '#')
                Advance();
        
            var extraHashes = text[extraHashesStart..position];
            tokens.Add(Token.Heading(headingText));
            tokens.Add(Token.Text(extraHashes));
        }
        else
        {
            tokens.Add(Token.Heading(headingText));
        }
    }

    private static EmphasisType AnalyzeUnderscore(string text, int position, int underscoreCount,
        ref bool italicOpen, ref bool boldOpen)
    {
        var prevChar = GetChar(text, position - 1);
        var nextChar = GetChar(text, position + underscoreCount);
        
        var prevIsLetter = char.IsLetter(prevChar);
        var nextIsLetter = char.IsLetter(nextChar);
        var prevIsDigit = char.IsDigit(prevChar);
        var nextIsDigit = char.IsDigit(nextChar);
        var prevIsSpace = IsWhitespaceOrStart(text, position - 1);
        var nextIsSpace = IsWhitespaceOrEnd(text, position + underscoreCount);
        var prevIsPunct = char.IsPunctuation(prevChar);
        var nextIsPunct = char.IsPunctuation(nextChar);
        var prevIsEscape = prevChar == '\\';
        
        if (underscoreCount == 2 && nextIsSpace)
        {
            var searchPos = position + underscoreCount;
            while (searchPos < text.Length && char.IsWhiteSpace(text[searchPos]))
            {
                searchPos++;
            }
            if (searchPos < text.Length && text[searchPos] == '_')
            {
                return EmphasisType.None;
            }
        }
        
        if (IsInvalidInnerUnderscore(text, position, underscoreCount, prevIsLetter, nextIsLetter) || prevIsDigit || nextIsDigit)
            return EmphasisType.None;

        return underscoreCount switch
        {
            1 => AnalyzeSingleUnderscore(prevIsLetter, prevIsPunct, prevIsSpace,
                nextIsLetter, nextIsSpace, nextIsPunct, prevIsEscape, 
                ref italicOpen),

            2 => AnalyzeDoubleUnderscore(prevIsSpace, prevIsPunct, prevChar,
                nextIsLetter, nextIsSpace, nextIsPunct,
                ref boldOpen),

            3 => AnalyzeTripleUnderscore(prevIsSpace, prevIsPunct,
                nextIsSpace, nextChar),

            _ => EmphasisType.None
        };
    }

    private static char GetChar(string text, int index) =>
        index >= 0 && index < text.Length ? text[index] : '\0';

    private static bool IsWhitespaceOrStart(string text, int index) =>
        index < 0 || char.IsWhiteSpace(GetChar(text, index));

    private static bool IsWhitespaceOrEnd(string text, int index) =>
        index >= text.Length || char.IsWhiteSpace(GetChar(text, index));

    private static bool IsInvalidInnerUnderscore(string text, int pos, int count, bool prevIsLetter, bool nextIsLetter)
    {
        if (!(prevIsLetter && nextIsLetter))
            return false;

        var searchPos = pos + count;
        var foundWhitespace = false;

        while (searchPos < text.Length)
        {
            var current = text[searchPos];
            if (current == ' ') foundWhitespace = true;
            if (current == '_')
            {
                if (foundWhitespace && IsLetterSurrounded(text, searchPos))
                    return true;
                break;
            }

            searchPos++;
        }

        return false;
    }

    private static bool IsLetterSurrounded(string text, int index)
    {
        var before = GetChar(text, index - 1);
        var after = GetChar(text, index + 1);
        return char.IsLetter(before) && char.IsLetter(after);
    }

    private static EmphasisType AnalyzeSingleUnderscore(
        bool prevIsLetter, bool prevIsPunct, bool prevIsSpace,
        bool nextIsLetter, bool nextIsSpace, bool nextIsPunct, bool prevIsEscape,
        ref bool italicOpen)
    {
        if (prevIsLetter || prevIsPunct)
        {
            italicOpen = !italicOpen;
            return italicOpen ? EmphasisType.ItalicStart : EmphasisType.ItalicEnd;
        }

        if ((prevIsSpace || prevIsPunct) && nextIsLetter || nextIsPunct)
        {
            italicOpen = true;
            return EmphasisType.ItalicStart;
        }

        if ((!prevIsLetter && !prevIsEscape) || (!nextIsSpace && !nextIsPunct))
            return EmphasisType.None;

        italicOpen = false;
        return EmphasisType.ItalicEnd;
    }

    private static EmphasisType AnalyzeDoubleUnderscore(
        bool prevIsSpace, bool prevIsPunct, char prevChar,
        bool nextIsLetter, bool nextIsSpace, bool nextIsPunct,
        ref bool boldOpen)
    {
        if ((prevIsSpace || prevIsPunct || prevChar == '\0') && nextIsLetter && !boldOpen)
        {
            boldOpen = true;
            return EmphasisType.BoldStart;
        }

        if (!boldOpen || (nextIsLetter && !nextIsSpace && !nextIsPunct))
            return EmphasisType.None;

        boldOpen = false;
        return EmphasisType.BoldEnd;
    }

    private static EmphasisType AnalyzeTripleUnderscore(
        bool prevIsSpace, bool prevIsPunct,
        bool nextIsSpace, char nextChar)
    {
        var canOpen = !nextIsSpace && nextChar != '\0';
        var canClose = !prevIsSpace && !prevIsPunct;

        return canOpen switch
        {
            true when !canClose => EmphasisType.BoldItalicStart,
            false when canClose => EmphasisType.BoldItalicEnd,
            _ => EmphasisType.None
        };
    }
    
    private static List<Token> ConvertUnmatchedTokensToText(List<Token> tokens)
    {
        var invalidIndices = FindInvalidTokenIndices(tokens);
        return ReplaceInvalidTokensWithText(tokens, invalidIndices);
    }

    private static HashSet<int> FindInvalidTokenIndices(List<Token> tokens)
    {
        var invalid = new HashSet<int>();

        var boldRanges = FindValidTagPairs(tokens, TokenType.BoldStart, TokenType.BoldEnd);
        var italicRanges = FindValidTagPairs(tokens, TokenType.ItalicStart, TokenType.ItalicEnd);

        foreach (var b in boldRanges)
        {
            foreach (var i in italicRanges.Where(i => IsCrossing(b.start, b.end, i.start, i.end)))
            {
                invalid.UnionWith([b.start, b.end, i.start, i.end]);
            }
        }

        var unpaired = FindUnpairedTokens(tokens, invalid);
        invalid.UnionWith(unpaired.Select(x => x.index));

        return invalid;
    }

    private static List<Token> ReplaceInvalidTokensWithText(List<Token> tokens, HashSet<int> invalidIndices)
    {
        return tokens
            .Select((t, i) => invalidIndices.Contains(i) ? Token.Text(t.Value) : t)
            .ToList();
    }

    private static Stack<(Token token, int index)> FindUnpairedTokens(List<Token> list, HashSet<int> hashSet)
    {
        var valueTuples = new Stack<(Token token, int index)>();

        for (var i = 0; i < list.Count; i++)
        {
            if (hashSet.Contains(i)) continue;

            var token = list[i];

            switch (token.Type)
            {
                case TokenType.BoldStart:
                case TokenType.ItalicStart:
                    valueTuples.Push((token, i));
                    break;

                case TokenType.BoldEnd:
                case TokenType.ItalicEnd:
                    if (valueTuples.Count == 0 || !IsMatching(valueTuples.Peek().token, token))
                        hashSet.Add(i);
                    else
                        valueTuples.Pop();
                    break;
            }
        }
        return valueTuples;
    }

    private static List<(int start, int end)> FindValidTagPairs(List<Token> tokens, TokenType startType,
        TokenType endType)
    {
        var pairs = new List<(int start, int end)>();
        var stack = new Stack<int>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Type == startType)
                stack.Push(i);
            else if (token.Type == endType && stack.Count > 0)
                pairs.Add((stack.Pop(), i));
        }

        return pairs;
    }

    private static bool IsMatching(Token open, Token close) =>
        (open.Type == TokenType.BoldStart && close.Type == TokenType.BoldEnd) ||
        (open.Type == TokenType.ItalicStart && close.Type == TokenType.ItalicEnd);

    private static bool IsCrossing(int start1, int end1, int start2, int end2) =>
        (start2 > start1 && start2 < end1 && end2 > end1) ||
        (start1 > start2 && start1 < end2 && end1 > end2);
}