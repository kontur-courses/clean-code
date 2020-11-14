using System.Collections.Generic;

namespace Markdown
{
    public abstract class Parser
    {
        protected readonly Stack<MachineState> States;
        protected readonly Stack<TextInfo> NestedTextInfos;
        protected MachineState State;
        protected TextInfo TextInfo;

        protected string Markdown;
        protected bool TextEnded;
        protected int BackslashCounter;
        protected int PreviousIndex;
        protected int WordStartIndex;

        private readonly HashSet<char> keySymbols = new HashSet<char> {'_', '#'};

        protected delegate void MachineState(int index);

        protected Parser()
        {
            TextInfo = new TextInfo();
            NestedTextInfos = new Stack<TextInfo>();
            States = new Stack<MachineState>();
        }

        protected bool ShouldEscaped(char symbol)
        {
            return keySymbols.Contains(symbol) && BackslashCounter % 2 != 0;
        }

        protected bool SymbolIsKey(char symbol)
        {
            return keySymbols.Contains(symbol);
        }

        protected void SetNewTextInfo(TextInfo newTextInfo)
        {
            TextInfo.AddContent(newTextInfo);
            NestedTextInfos.Push(TextInfo);
            TextInfo = newTextInfo;
        }

        protected void SetNewState(MachineState newState)
        {
            States.Push(State);
            State = newState;
        }
    }
}