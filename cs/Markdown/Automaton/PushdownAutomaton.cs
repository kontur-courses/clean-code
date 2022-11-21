using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    internal class PushdownAutomaton : IPushdownAutomaton
    {
        public TransitionFunction TransitionFunction{ get; }
        public Dictionary<string, List<Rule>> rules;
        private readonly Stack<string> stack = new();

        public PushdownAutomaton(TransitionFunction transitionFunction, Dictionary<string, List<Rule>> rules)
        {
            TransitionFunction = transitionFunction;
            this.rules = rules;
        }

        // Запустит автомат, вернёт true, если входные данные успешно прочитаны, иначе false
        public bool Run(char[] tokens)
        {
            var currentState = "0";
            stack.Push("%"); // Маркер дна

            foreach (var token in tokens)
            {
                var argument = new TransitionFunctionArgument(
                    currentState, token.ToString(), stack.Pop());
                bool ruleHasBeenUsed = false;

                if (rules.ContainsKey(currentState))
                {
                    foreach (var rule in rules[currentState])
                    {
                        var value = rule.GetValue(argument);
                        if (value is null) continue;
                        
                        currentState = value.NewState;
                        AddNewElement(stack, value.NewStackElements);
                        ruleHasBeenUsed = true;
                    }
                }
                if (!ruleHasBeenUsed)
                {
                    //Console.WriteLine(TransitionFunction.Transitions.Comparer);
                    var val = TransitionFunction.Transitions[argument];
                    currentState = val.NewState;
                    AddNewElement(stack, val.NewStackElements);
                }
            }

            return true;
        }

        // Преобразует текущие элементы стека в строку и вернёт её
        public string GetAutomatonMemory()
        {
            //throw new NotImplementedException();
            var buidler = new StringBuilder();
            
            foreach (var element in stack.ToArray().Reverse())
            {
                buidler.Append(element);
            }

            return buidler.ToString();
        }

        private void AddNewElement(Stack<string> stack, string[] elements)
        {
            foreach (var element in elements.Reverse())
            {
                stack.Push(element);
            }
        }
    }
}
