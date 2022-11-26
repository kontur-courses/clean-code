namespace Markdown.States;

public static class TransitionExtensions
{
    public static void ReadInput(this State state)
    {
        var input = state.Input;
        if (input.Length == 2 && input[0] == '\\')
            input = input[1..2];
        state.ValueBuilder.Append(input);
        state.MoveNext();
    }
}