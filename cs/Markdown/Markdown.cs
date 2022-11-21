using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Automaton;
using Markdown.Automaton.Interfaces;

namespace Markdown
{
    internal class Markdown : IRenderer
    {
        private void AddTransitions(
            string currentState,
            string inputToken,
            string stackTop,
            string newState,
            string[] newStackElements,
            Dictionary<TransitionFunctionArgument, TransitionFunctionValue> dict)
        {
            var arg = new TransitionFunctionArgument(currentState, inputToken, stackTop);
            var value = new TransitionFunctionValue(newState, newStackElements);
            dict.Add(arg, value);

        }
        private Dictionary<TransitionFunctionArgument, TransitionFunctionValue> GetTransitions()
        {
            var comparer = new TransitionFunctionArgumentComparer();
            var transitions = new Dictionary<TransitionFunctionArgument, TransitionFunctionValue>(comparer);
            
            AddTransitions(
                "3", "_", "<em>", "0", new[] {@"<\em>"}, transitions);
          
            AddTransitions(
                "1", "_", "<em>", "2", new [] {"<es>"}, transitions);

            return transitions;
        }

        //private bool AllPredicatesAreTrue(
        //    Predicate<ITransitionFunctionArgument>[] predicates,
        //    ITransitionFunctionArgument argument)
        //{
        //    foreach (var predicate in predicates)
        //    {
        //        if (!predicate(argument))
        //            return false;
        //    }

        //    return true;
        //}

        private void AddRule(
            Dictionary<string, List<Rule>> dict,
            Func<ITransitionFunctionArgument, ITransitionFunctionValue> func,
            string currentState )
        {
            var rule = new Rule(func);
            
            if (dict.ContainsKey(currentState))
                dict[currentState].Add(rule);
            else 
                dict.Add(currentState, new List<Rule>() {rule});
        }

        //private Func<ITransitionFunctionArgument, ITransitionFunctionValue?> GetFunc(
        //    Predicate<ITransitionFunctionArgument>[] predicates,
        //    string newState,
        //    string[] newStackElements)
        //{
        //    return (ITransitionFunctionArgument argument) =>
        //        AllPredicatesAreTrue(predicates, argument) 
        //            ? new TransitionFunctionValue(newState, newStackElements) 
        //            : null;
        //}

        private Dictionary<string, List<Rule>> GetRules()
        {
            var rulesDictionary = new Dictionary<string, List<Rule>>();
            
            var func1 = (ITransitionFunctionArgument argument) =>
                Char.IsLetter(char.Parse(argument.InputToken))
                    ? new TransitionFunctionValue(
                         "0", new string[] { argument.InputToken, argument.StackTop })
                    : null;
            AddRule(rulesDictionary, func1, "0");

            var func2 = (ITransitionFunctionArgument argument) =>
                argument.InputToken == "_" 
                    ? new TransitionFunctionValue(
                        "1", new string[] {"<em>", argument.StackTop})
                    : null;
            AddRule(rulesDictionary, func2, "0");
            
            var func3 = (ITransitionFunctionArgument argument) =>
                Char.IsLetter(char.Parse(argument.InputToken)) &&
                argument.StackTop == "<em>"
                    ? new TransitionFunctionValue(
                        "3", new string[]
                        {
                            "<em>",
                            argument.InputToken,
                            "<em>"
                        })
                    : null;
            AddRule(rulesDictionary, func3, "1");

            var func4 = (ITransitionFunctionArgument argument) =>
                Char.IsLetter(char.Parse(argument.InputToken)) &&
                argument.StackTop == "<em>"
                    ? new TransitionFunctionValue(
                        "3", new string[]
                        {
                            "<em>",
                            argument.InputToken
                        })
                    : null;
            AddRule(rulesDictionary, func4, "3");

            return rulesDictionary;
        }

        public string Render(string text)
        {
            var transitions = GetTransitions();
            var transitionFunction = new TransitionFunction(transitions); 
            var rules = GetRules(); 
            var automaton = new PushdownAutomaton(transitionFunction, rules);
            automaton.Run(text.ToCharArray());

            return automaton.GetAutomatonMemory();
        }
    }
}
