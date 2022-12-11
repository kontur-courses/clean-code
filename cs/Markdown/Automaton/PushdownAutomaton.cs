using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    internal class PushdownAutomaton : IPushdownAutomaton
    {
        private readonly Stack<string> mainStack = new();
        private readonly Stack<int> mainStackIndices = new();
        private int generalCounter = -1;
        private int internalCounter = -1;

        public int IndexOffset { get; }
        public List<IAutomatonPart> Parts { get; }
        
        public PushdownAutomaton(List<IAutomatonPart> parts, int indexOffset = 0)
        {
            Parts = parts;
            IndexOffset = indexOffset;
        }

        public void Run(string text)
        {
            text += "%"; // Маркер конца
            var tokens = text.Select(c => c.ToString()).ToArray();
            var mainPart = FindMainPart();

            RunPart(mainPart, tokens);
        }

        public void Run(string[] tokens)
        {
            var mainPart = FindMainPart();
            RunPart(mainPart, tokens);
        }

        public void Dispose()
        {
            ClearMemory();
            generalCounter = -1;
            internalCounter = -1;
        }

        public string[] GetAutomatonMemory()
        {
            var list = new List<string>();

            foreach (var element in mainStack.ToArray().Reverse())
            {
                if (element != "")
                    list.Add(element);
            }

            return list.ToArray();
        }

        public int[] GetMainStackIndicesAutomatonMemory()
        {
            var list = new List<int>();
            foreach (var element in mainStackIndices.ToArray().Reverse())
            {
                list.Add(element);
            }
            return list.ToArray();
        }

        private IAutomatonPart FindMainPart()
        {
            return Parts.Find(p => p.IsMainPart)
                ?? throw new ArgumentException("One of the parts of the automaton must be the main");
        }

        private IAutomatonPart FindPart(AutomatonPartStarter starter)
        {
            return Parts.Find(p => p.Name == starter.PartName)
                   ?? throw new ArgumentException("The specified automaton part was not found in the automaton");
        }

        private ITransitionFunctionValue? GetValue(ITransitionFunctionArgument argument, IAutomatonState currentState)
        {
            currentState.TransitionFunction.Transitions.TryGetValue(argument, out var value);

            if (value != null) 
                return value;
            
            if (argument.InputToken == "%") 
                return null;
                
            // Если мы не нашли переход по определенному StackTop, попробуем найти по любому другому.
            // В конфигурации автоматов, если текущее состояние стека не важно, оно указывается как null  
            argument.StackTop = null;
            value = currentState.TransitionFunction.Transitions[argument];

            return value;
        }

        private string[] GetNewTokens(string[] tokens)
        {
            var newArrayLength = tokens.Length - generalCounter - 1;
            var newTokens = new string[newArrayLength];
            Array.Copy(tokens, generalCounter + 1, newTokens, 0, newArrayLength);
            return newTokens;
        }

        private void UpdateCounters(int i)
        {
            internalCounter = i;
            generalCounter++;
        }

        private void RunPart(IAutomatonPart part, string[] tokens, string parentName = null)
        {
            var currentStateIndex = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                UpdateCounters(i);
                
                mainStack.TryPeek(out var stackTop);
                var argument = new TransitionFunctionArgument(currentStateIndex, StandardizeToken(tokens[i]), stackTop);
                var value = GetValue(argument, currentState: part.States[currentStateIndex]);
                
                if (value == null) 
                    break;

                CheckStackTop(argument);

                if (value is TransitionFunctionValue trFuncValue) // Обычное значение
                {
                    currentStateIndex = trFuncValue.NewState;
                    AddNewElements(trFuncValue.NewStackElements, generalCounter);
                    if (trFuncValue.NewState == -1) 
                        return;
                }
                else if (value is AutomatonPartStarter starter) // Переход в новую часть автомата
                { 
                    AddNewElements(starter.NewStackElements, generalCounter);
                    RunPart(FindPart(starter), GetNewTokens(tokens), starter.PartName);

                    i += internalCounter + 1;
                    currentStateIndex = starter.ParentPartStateIndexToReturn;
                }
            }
        }

        private void CheckStackTop(ITransitionFunctionArgument argument)
        {
            if (argument.StackTop != null)
                PopMainStack();
        }

        private void PopMainStack()
        {
            mainStack.Pop();
            mainStackIndices.Pop();
        }

        private void ClearMemory()
        {
            mainStack.Clear();
            mainStackIndices.Clear();
        }

        private static string StandardizeToken(string token)
        {
            if (token.Length == 1)
            {
                var symbol = token[0];

                switch (symbol)
                {
                    case '%':
                        return "%";
                    case '\\':
                        return "\\";
                    case ' ':
                        return " ";
                    case var c when char.IsDigit(c):
                        return "1";
                    case var c when char.IsLetter(c) || char.IsPunctuation(c) && token != "_" && token != "#":
                        return "K";
                }
            }

            return token;
        }

        private void AddNewElements(string[] elements, int i)
        {
            foreach (var element in elements.Reverse())
            {
                AddNewElement(element, i++);
            }
        }

        private void AddNewElement(string element, int i)
        {
            if (element == "") return;

            mainStack.Push(element);
            mainStackIndices.Push(i);
        }
    }
}
