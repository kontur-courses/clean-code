using MarkupConverter.Parsers.BlockParsers.OpenBlocks;

namespace MarkupConverter.Parsers;

public class ParsingContext
{
    private readonly Stack<IOpenBlock> stack = new();

    public IOpenBlock CurrentBlock =>
        stack.TryPeek(out var block)
            ? block
            : throw new InvalidOperationException("No open blocks in the parsing context.");

    public bool HasOpenBlocks => stack.Count > 0;

    public void Push(IOpenBlock block)
    {
        stack.Push(block);
    }

    public IOpenBlock Pop()
    {
        if (!HasOpenBlocks)
            throw new InvalidOperationException("Cannot pop from an empty parsing context.");

        return stack.Pop();
    }
}