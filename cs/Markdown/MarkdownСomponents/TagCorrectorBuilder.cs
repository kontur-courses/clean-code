using System.Collections.Generic;
using Markdown.Automaton;
using Markdown.Automaton.Interfaces;

namespace Markdown.MarkdownСomponents
{
    internal class TagCorrectorBuilder
    {
        private static string mainPartName = "Main Part";
        private static readonly TransitionFunctionArgumentComparer comparer = new();

        public static PushdownAutomaton BuildTagCorrector()
        {
            var mainPart = BuildTagCorrectorMainPart();
            return new PushdownAutomaton(new List<IAutomatonPart>() { mainPart });
        }

        #region MainPart states configuration

        private static AutomatonPart BuildTagCorrectorMainPart()
        {
            var states = GetMainPartStates();
            return new AutomatonPart(mainPartName, states, isMainPart: true);
        }

        private static List<IAutomatonState> GetMainPartStates()
        {
            return new List<IAutomatonState> 
            { 
                GetMainPartState0(), GetMainPartState1(), GetMainPartState2(), GetMainPartState3(),
            };
        }

        private static IAutomatonState GetMainPartState0()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, "<em>", null),
                    new TransitionFunctionValue(2, "<em>")
                },
                {
                    new TransitionFunctionArgument(0, "<strong>", null),
                    new TransitionFunctionValue(1, "<strong>")
                },



                {
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "1", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, " ", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, @"\", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "_", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "__", null),
                    new TransitionFunctionValue(0, "")
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
                    new TransitionFunctionArgument(1, "<em>", null),
                    new TransitionFunctionValue(3, "<em>")
                },
                {
                    new TransitionFunctionArgument(1, @"<\strong>", "<strong>"),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"%", "<strong>"),
                    new TransitionFunctionValue(0, new[] { "_", "_"})
                },


                {
                    new TransitionFunctionArgument(1, "K", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, "1", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, " ", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, @"\", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, "_", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, "__", null),
                    new TransitionFunctionValue(1, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(1, transitionFunction);
        }

        private static IAutomatonState GetMainPartState2()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(2, "%", "<em>"),
                    new TransitionFunctionValue(0, "_")
                },
                {
                    new TransitionFunctionArgument(2, @"<\em>", "<em>"),
                    new TransitionFunctionValue(0, "")
                },

                {
                    new TransitionFunctionArgument(2, "K", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "1", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, " ", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, @"\", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "_", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "__", null),
                    new TransitionFunctionValue(2, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(2, transitionFunction);
        }
        private static IAutomatonState GetMainPartState3()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(3, "%", "<em>"),
                    new TransitionFunctionValue(1, "_")
                },
                {
                    new TransitionFunctionArgument(3, @"<\em>", "<em>"),
                    new TransitionFunctionValue(1, "")
                },


                {
                    new TransitionFunctionArgument(3, "K", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, "1", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, " ", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, @"\", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, "_", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, "__", null),
                    new TransitionFunctionValue(3, "")
                }

            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(3, transitionFunction);
        }

        #endregion
    }
}