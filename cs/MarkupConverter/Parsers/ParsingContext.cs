using MarkupConverter.Parsers.BlockParsers.OpenBlocks;

namespace MarkupConverter.Parsers;

public class ParsingContext
{
    private readonly Stack<IOpenBlock?> stack = new();

    public IOpenBlock? CurrentBlock =>
        stack.Count > 0 ? stack.Peek() : null;

    public void Push(IOpenBlock? block)
    {
        stack.Push(block);
    }

    public IOpenBlock? Pop()
    {
        return stack.Count > 0 ? stack.Pop() : null;
    }

    public bool HasOpenBlocks => stack.Count > 0;
}