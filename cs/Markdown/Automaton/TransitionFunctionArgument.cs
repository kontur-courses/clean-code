using System.Collections.Generic;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    public class TransitionFunctionArgument : ITransitionFunctionArgument
    {
        public int CurrentState { get; }
        public string InputToken { get; }
        public string? StackTop { get; set; }

        public TransitionFunctionArgument(int currentState, string inputToken, string? stackTop)
        {
            CurrentState = currentState;
            InputToken = inputToken;
            StackTop = stackTop;
        }
    }

    public class TransitionFunctionArgumentComparer : EqualityComparer<ITransitionFunctionArgument>
    {
        public override bool Equals(ITransitionFunctionArgument? arg1, ITransitionFunctionArgument? arg2)
        {
            if (arg1 == null && arg2 == null)
                return true;

            if (arg1 == null || arg2 == null)
                return false;

            if (arg1.InputToken == arg2.InputToken &&
                arg1.CurrentState == arg2.CurrentState &&
                arg1.StackTop == arg2.StackTop)
                return true;

            return false;
        }

        public override int GetHashCode(ITransitionFunctionArgument argument)
        {
            int hCode = argument.InputToken.GetHashCode() ^
                        argument.CurrentState.GetHashCode();
            return hCode.GetHashCode();
        }
    }

}
