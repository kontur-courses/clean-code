using Markdown.States;

namespace Markdown;

public interface ITracer
{
    public void TraceState(State state);

    public void TraceTransition(Transition transition);
}