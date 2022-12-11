using System.Collections.Generic;

namespace Markdown.Automaton.Interfaces
{
    public interface IAutomatonPart
    {
        public string Name { get; }
        public List<IAutomatonState> States { get; }
        public bool IsMainPart { get; }
    }
}
