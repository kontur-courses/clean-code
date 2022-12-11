using System.Collections.Generic;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    internal class AutomatonPart : IAutomatonPart
    {
        public string Name { get; }
        public List<IAutomatonState> States { get; }
        public bool IsMainPart { get; }

        public AutomatonPart(string name, List<IAutomatonState> states, bool isMainPart = false)
        {
            Name = name;
            States = states;
            IsMainPart = isMainPart;
        }
    }
}
