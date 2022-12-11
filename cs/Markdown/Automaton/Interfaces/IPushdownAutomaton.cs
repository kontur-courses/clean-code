using System.Collections.Generic;

namespace Markdown.Automaton.Interfaces
{
    public interface IPushdownAutomaton
    {
        public List<IAutomatonPart> Parts { get; }
        private static Stack<string> Stack { get; set; }
        public void Run(string tokens);
    }
}
