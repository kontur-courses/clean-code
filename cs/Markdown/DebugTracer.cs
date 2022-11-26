using Markdown.States;

namespace Markdown;

public class DebugTracer : ITracer
{
    private readonly TextWriter writer;

    public DebugTracer(TextWriter writer)
    {
        this.writer = writer;
    }

    public void TraceState(State state)
    {
        writer.WriteLine();
        writer.WriteLine(state.ToString());
        writer.WriteLine();
    }

    public void TraceTransition(Transition transition)
    {
        writer.WriteLine();
        writer.WriteLine(transition.ToString());
        writer.WriteLine();
    }
}