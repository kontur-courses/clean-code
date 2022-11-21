using System.Collections.Generic;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    public class TransitionFunctionArgument : ITransitionFunctionArgument
    {
        public string CurrentState { get; }
        public string InputToken { get; }
        public string StackTop { get; }

        public TransitionFunctionArgument(string currentState, string inputToken, string stackTop)
        {
            CurrentState = currentState;
            InputToken = inputToken;
            StackTop = stackTop;
        }
    }

    public class TransitionFunctionArgumentComparer : EqualityComparer<TransitionFunctionArgument>
    {
        public override bool Equals(TransitionFunctionArgument? arg1, TransitionFunctionArgument? arg2)
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

        public override int GetHashCode(TransitionFunctionArgument argument)
        {
            int hCode = argument.InputToken.GetHashCode() ^
                        argument.CurrentState.GetHashCode() ^
                        argument.StackTop.GetHashCode();
            return hCode.GetHashCode();
        }
    }

}
