using Markdown.Automaton;
using Markdown.Automaton.Interfaces;
using System.Collections.Generic;

namespace Markdown.MarkdownСomponents
{
    internal class DifferentWordSelectionHandlerBuilder
    {
        private static string mainPartName = "Main Part";
        private static readonly TransitionFunctionArgumentComparer comparer = new();

        public static PushdownAutomaton BuildDifferentWordSelectionHandler()
        {
            var mainPart = BuildDifferentWordSelectionHandlerMainPart();
            return new PushdownAutomaton(new List<IAutomatonPart>() { mainPart });
        }

        private static AutomatonPart BuildDifferentWordSelectionHandlerMainPart()
        {
            var states = GetMainPartStates();
            return new AutomatonPart(mainPartName, states, isMainPart: true);
        }

        private static List<IAutomatonState> GetMainPartStates()
        {
            return new List<IAutomatonState>
            {
                GetMainPartState0(), GetMainPartState1(), GetMainPartState2(), GetMainPartState3(), GetMainPartState4(),
                GetMainPartState5(), GetMainPartState6(), GetMainPartState7(), GetMainPartState8(), GetMainPartState9()
            };
        }

        private static IAutomatonState GetMainPartState0()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, " ", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "1", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "\\", null),
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
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(0, "<em>", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(0, "<strong>", null),
                    new TransitionFunctionValue(6, "")
                },
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(0, transitionFunction);
        }

        private static IAutomatonState GetMainPartState1()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(1, " ", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, @"<\em>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "\\", null),
                    new TransitionFunctionValue(1, "")
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
                    new TransitionFunctionArgument(1, "__", null),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(1, "<strong>", null),
                    new TransitionFunctionValue(2, "<strong>")
                },
                {
                    new TransitionFunctionArgument(1, "<em>", null),
                    new TransitionFunctionValue(4, "<em>")
                },
                {
                    new TransitionFunctionArgument(1, "_", null),
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
                    new TransitionFunctionArgument(2, " ", null),
                    new TransitionFunctionValue(2, " ")
                },
                {
                    new TransitionFunctionArgument(2, " ", " "),
                    new TransitionFunctionValue(2, " ")
                },
                {
                    new TransitionFunctionArgument(2, "\\", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "_", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "__", null),
                    new TransitionFunctionValue(2, "")
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
                    new TransitionFunctionArgument(2, "<em>", null),
                    new TransitionFunctionValue(3, "<em>")
                },
                {
                    new TransitionFunctionArgument(2, @"<\strong>", "<strong>"),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(2, @"<\strong>", " "),
                    new TransitionFunctionValue(1, @"<\strong>")
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
                    new TransitionFunctionArgument(3, " ", null),
                    new TransitionFunctionValue(3, " ")
                },
                {
                    new TransitionFunctionArgument(3, "\\", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, " ", " "),
                    new TransitionFunctionValue(3, " ")
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
                    new TransitionFunctionArgument(3, "__", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, "_", null),
                    new TransitionFunctionValue(3, "")
                },
                {
                    new TransitionFunctionArgument(3, @"<\em>", "<em>"),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(3, @"<\em>", " "),
                    new TransitionFunctionValue(2, @"<\em>")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(3, transitionFunction);
        }

        private static IAutomatonState GetMainPartState4()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(4, " ", null),
                    new TransitionFunctionValue(4, " ")
                },
                {
                    new TransitionFunctionArgument(4, "\\", null),
                    new TransitionFunctionValue(4, "")
                },
                {
                    new TransitionFunctionArgument(4, " ", " "),
                    new TransitionFunctionValue(4, " ")
                },
                {
                    new TransitionFunctionArgument(4, "_", null),
                    new TransitionFunctionValue(4, "")
                },
                {
                    new TransitionFunctionArgument(4, "__", null),
                    new TransitionFunctionValue(4, "")
                },
                {
                    new TransitionFunctionArgument(4, "K", null),
                    new TransitionFunctionValue(4, "")
                },
                {
                    new TransitionFunctionArgument(4, "1", null),
                    new TransitionFunctionValue(4, "")
                },
                {
                    new TransitionFunctionArgument(4, @"<\em>", "<em>"),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(4, @"<\em>", " "),
                    new TransitionFunctionValue(1, @"<\em>")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(4, transitionFunction);
        }
        
        private static IAutomatonState GetMainPartState5()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(5, " ", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, "_", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, "__", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, "\\", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, " ", " "),
                    new TransitionFunctionValue(5, " ")
                },
                {
                    new TransitionFunctionArgument(5, "K", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, "1", null),
                    new TransitionFunctionValue(5, "")
                },
                {
                    new TransitionFunctionArgument(5, @"<\em>", null),
                    new TransitionFunctionValue(0, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(5, transitionFunction);
        }

        private static IAutomatonState GetMainPartState6()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(6, " ", null),
                    new TransitionFunctionValue(6, "")
                },
                {
                    new TransitionFunctionArgument(6, "\\", null),
                    new TransitionFunctionValue(6, "")
                },
                {
                    new TransitionFunctionArgument(6, "K", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(6, @"<\strong>", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(6, @"<em>", null),
                    new TransitionFunctionValue(9, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(6, transitionFunction);
        }

        private static IAutomatonState GetMainPartState7()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(7, "<em>", null),
                    new TransitionFunctionValue(8, "<em>")
                },
                {
                    new TransitionFunctionArgument(7, "\\", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(7, " ", null),
                    new TransitionFunctionValue(6, "")
                },
                {
                    new TransitionFunctionArgument(7, "K", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(7, "1", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(7, "_", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(7, "__", null),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(7, @"<\strong>", null),
                    new TransitionFunctionValue(0, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(7, transitionFunction);
        }
        private static IAutomatonState GetMainPartState8()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(8, " ", null),
                    new TransitionFunctionValue(8, " ")
                },
                {
                    new TransitionFunctionArgument(8, "\\", null),
                    new TransitionFunctionValue(8, "")
                },
                {
                    new TransitionFunctionArgument(8, " ", " "),
                    new TransitionFunctionValue(8, " ")
                },
                {
                    new TransitionFunctionArgument(8, "_", null),
                    new TransitionFunctionValue(8, "")
                },
                {
                    new TransitionFunctionArgument(8, "__", null),
                    new TransitionFunctionValue(8, "")
                },
                {
                    new TransitionFunctionArgument(8, "K", null),
                    new TransitionFunctionValue(8, "")
                },
                {
                    new TransitionFunctionArgument(8, "1", null),
                    new TransitionFunctionValue(8, "")
                },
                {
                    new TransitionFunctionArgument(8, @"<\em>", "<em>"),
                    new TransitionFunctionValue(7, "")
                },
                {
                    new TransitionFunctionArgument(8, @"<\em>", " "),
                    new TransitionFunctionValue(7, @"<\em>")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(8, transitionFunction);
        }

        private static IAutomatonState GetMainPartState9()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(9, " ", null),
                    new TransitionFunctionValue(9, "")
                },
                {
                    new TransitionFunctionArgument(9, "\\", null),
                    new TransitionFunctionValue(9, "")
                },
                {
                    new TransitionFunctionArgument(9, " ", " "),
                    new TransitionFunctionValue(9, " ")
                },
                {
                    new TransitionFunctionArgument(9, "K", null),
                    new TransitionFunctionValue(9, "")
                },
                {
                    new TransitionFunctionArgument(9, "1", null),
                    new TransitionFunctionValue(9, "")
                },
                {
                    new TransitionFunctionArgument(9, @"<\em>", null),
                    new TransitionFunctionValue(6, "")
                },
                {
                    new TransitionFunctionArgument(9, "_", null),
                    new TransitionFunctionValue(9, "")
                },
                {
                    new TransitionFunctionArgument(9, "__", null),
                    new TransitionFunctionValue(9, "")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(9, transitionFunction);
        }
    }
}
