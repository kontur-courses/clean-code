namespace Markdown.Lexer
{
    internal interface ISequence<T>
    {
        int Location { get; }
        bool End { get; }
        T GetNext();
        T Peek();
    }
}