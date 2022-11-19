using Markdown.PushdownAutomaton.Interfaces;
using Markdown.Token;

namespace Markdown.PushdownAutomaton
{
    internal class PushdownAutomaton : IPushdownAutomaton
    {
        public IPushdownAutomatonStateTransitionTable transitionTable { get; }
        private readonly Stack<IToken> stack;

        public PushdownAutomaton(
            PushdownAutomatonStateTransitionTable transitionTable,
            Stack<IToken> stack)
        {
            this.transitionTable = transitionTable;
            this.stack = stack;
        }

        // Запустит автомат, вернёт true, если входные данные успешно прочитаны, иначе false
        public bool Run(IToken[] tokens)
        {
            throw new NotImplementedException();
        }

        // Преобразует текущие элементы стека в строку и вернёт её
        public string GetAutomatonMemory()
        {
            throw new NotImplementedException();
        }
    }
}
