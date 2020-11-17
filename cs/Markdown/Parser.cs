using System.Collections.Generic;

namespace Markdown
{
    public abstract class Parser
    {
        protected readonly Stack<MachineState> States;
        protected readonly Stack<TagInfo> NestedTagInfos;
        protected MachineState State;
        protected TagInfo TagInfo;

        protected string Markdown;
        protected bool TextEnded;
        protected int BackslashCounter;
        protected int PreviousIndex;

        protected HashSet<char> keySymbols;

        protected delegate void MachineState(int index);

        protected Parser()
        {
            keySymbols = new HashSet<char>();
            TagInfo = new TagInfo();
            NestedTagInfos = new Stack<TagInfo>();
            States = new Stack<MachineState>();
        }

        protected bool ShouldEscaped(char symbol)
        {
            return keySymbols.Contains(symbol) && BackslashCounter % 2 != 0;
        }

        protected void SetNewTagInfo(TagInfo newTagInfo)
        {
            TagInfo.AddContent(newTagInfo);
            NestedTagInfos.Push(TagInfo);
            TagInfo = newTagInfo;
        }

        protected void SetNewState(MachineState newState)
        {
            States.Push(State);
            State = newState;
        }
    }
}