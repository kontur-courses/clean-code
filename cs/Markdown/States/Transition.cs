namespace Markdown.States;

public abstract class Transition
{
    public abstract bool When(State state);

    public abstract void Do(State state);
}