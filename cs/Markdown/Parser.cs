using System.Collections.Generic;

namespace Markdown
{
    public abstract class Parser
    {
        protected readonly Stack<MachineState> States;
        protected readonly Stack<TagInfo> NestedTextInfos;
        protected MachineState State;
        protected TagInfo TagInfo;

        protected string Markdown;
        protected bool TextEnded;
        protected int BackslashCounter;
        protected int PreviousIndex;

        private readonly HashSet<char> keySymbols = new HashSet<char> {'_', '#'};

        protected delegate void MachineState(int index);

        protected Parser()
        {
            TagInfo = new TagInfo();
            NestedTextInfos = new Stack<TagInfo>();
            States = new Stack<MachineState>();
        }

        protected bool ShouldEscaped(char symbol)
        {
            return keySymbols.Contains(symbol) && BackslashCounter % 2 != 0;
        }

        protected void SetNewTextInfo(TagInfo newTagInfo)
        {
            TagInfo.AddContent(newTagInfo);
            NestedTextInfos.Push(TagInfo);
            TagInfo = newTagInfo;
        }

        protected void SetNewState(MachineState newState)
        {
            States.Push(State);
            State = newState;
        }
    }
}