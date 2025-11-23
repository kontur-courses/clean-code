using System;

namespace Markdown;

public class TextCursor
{
    private readonly string input;
    public int Position { get; private set; }
    public int Length => input.Length;
    public bool End => Position >= input.Length;

    public TextCursor(string input)
    {
        this.input = input ?? string.Empty;
        Position = 0;
    }

    public char Current => End ? '\0' : input[Position];

    public bool IsNewLine()
    {
        return Position == 0 || input[Position - 1] == '\n';
    }

    public char Peek(int offset = 1)
    {
        var idx = Position + offset;
        return idx >= 0 && idx < input.Length ? input[idx] : '\0';
    }

    public void Move(int count = 1) =>
        Position = Math.Min(Position + count, input.Length); //todo: подумать, может лучше trymove

    public void Revert(int mark) => Position = mark;

    public string Slice(int start, int end)
        => input.Substring(start, Math.Max(0, end - start));
    
    public bool Matches(string text)
    {
        if (text == null || Position + text.Length > input.Length) 
            return false;
        
        return !text.Where((t, i) => input[Position + i] != t).Any();
    }

}