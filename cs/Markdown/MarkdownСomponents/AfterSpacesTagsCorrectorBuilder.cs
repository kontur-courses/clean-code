using System.Collections.Generic;
using Markdown.Automaton;
using Markdown.Automaton.Interfaces;

namespace Markdown.MarkdownСomponents
{
    internal class AfterSpacesTagsCorrectorBuilder
    {
        private static string mainPartName = "Main Part";
        private static readonly TransitionFunctionArgumentComparer comparer = new();

        public static PushdownAutomaton BuildAfterSpacesTagsCorrector()
        {
            var mainPart = BuildAfterSpacesTagsCorrectorMainPart();
            return new PushdownAutomaton(new List<IAutomatonPart>() { mainPart });
        }

        private static AutomatonPart BuildAfterSpacesTagsCorrectorMainPart()
        {
            var states = GetMainPartStates();
            return new AutomatonPart(mainPartName, states, isMainPart: true);
        }

        private static List<IAutomatonState> GetMainPartStates()
        {
            return new List<IAutomatonState>
            {
                GetMainPartState0(), GetMainPartState1()
            };
        }

        private static IAutomatonState GetMainPartState0()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "_", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "__", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "1", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, @"\", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "<em>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, @"<\em>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "<strong>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, @"<\strong>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, @" ", null),
                    new TransitionFunctionValue(1, " ")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(0, transitionFunction);
        }

        private static IAutomatonState GetMainPartState1()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(1, "K", " "),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "1", " "),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"\", " "),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "_", " "),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "__", " "),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "K", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "1", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "_", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "__", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"\", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, " ", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "<em>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"<\em>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "<strong>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"<\strong>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"<\em>", " "),
                    new TransitionFunctionValue(0, @"<\em>")
                },
                {
                    new TransitionFunctionArgument(1, @"<\strong>", " "),
                    new TransitionFunctionValue(0, @"<\strong>")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(1, transitionFunction);
        }
    }
}
