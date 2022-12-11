using System.Collections.Generic;
using Markdown.Automaton;
using Markdown.Automaton.Interfaces;

namespace Markdown.MarkdownСomponents
{
    internal static class MarkdownHandlerBuilder
    {
        private static string mainPartName = "Main Part";
        private static string escapePartName = "Escape Part";
        private static string italicsPartName = "Italics Part";
        private static TransitionFunctionArgumentComparer comparer = new();

        public static PushdownAutomaton BuildMarkdownHandler()
        {
            var mainPart = BuildMarkdownHandlerMainPart();
            var escapePart = BuildMarkdownHandlerEscapePart();
            var italicsPart = BuildMarkdownHandlerItalicsPart();
            return new PushdownAutomaton(new List<IAutomatonPart>() { mainPart, escapePart, italicsPart });
        }

        #region MainPart states configuration

        private static AutomatonPart BuildMarkdownHandlerMainPart()
        {
            var states = GetMainPartStates();
            return new AutomatonPart(mainPartName, states, isMainPart: true);
        }

        private static List<IAutomatonState> GetMainPartStates()
        {
            return new List<IAutomatonState> { GetMainPartState0(), GetMainPartState1(), GetMainPartState2(), GetMainPartState3() };
        }

        private static IAutomatonState GetMainPartState0()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, "1", null),
                    new TransitionFunctionValue(0, "1")
                },
                {
                    new TransitionFunctionArgument(0, "%", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(0, "K")
                },
                {
                    new TransitionFunctionArgument(0, " ", null),
                    new TransitionFunctionValue(0, " ")
                },
                {
                    new TransitionFunctionArgument(0, "_", null),
                    new TransitionFunctionValue(1, "_")
                },
                {
                    new TransitionFunctionArgument(0, "\\", null),
                    new AutomatonPartStarter(escapePartName, mainPartName, 0, "\\")
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
                    new TransitionFunctionArgument(1, "1", "_"),
                    new TransitionFunctionValue(0, new[] {"1", "_"})
                },
                {
                    new TransitionFunctionArgument(1, "%", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(1, "_", "_"),
                    new TransitionFunctionValue(2, "<strong>")
                },
                {
                    new TransitionFunctionArgument(1, " ", null),
                    new TransitionFunctionValue(0, " ")
                },
                {
                    new TransitionFunctionArgument(1, "\\", null),
                    new AutomatonPartStarter(escapePartName, mainPartName, 0, "\\")
                },
                {
                    new TransitionFunctionArgument(1, "K", "_"),
                    new AutomatonPartStarter(italicsPartName, mainPartName, 0,
                        new []{ "K", "<em>" })
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
                    new TransitionFunctionArgument(2, "1", null),
                    new TransitionFunctionValue(2, "1")
                },
                {
                    new TransitionFunctionArgument(2, "K", null),
                    new TransitionFunctionValue(2, "K")
                },
                {
                    new TransitionFunctionArgument(2, " ", "K"),
                    new TransitionFunctionValue(2, new[] { " ", "K"})
                },
                {
                    new TransitionFunctionArgument(2, " ", " "),
                    new TransitionFunctionValue(2, new [] {" ", " "})
                },
                {
                    new TransitionFunctionArgument(2, " ", "1"),
                    new TransitionFunctionValue(2, " ")
                },
                {
                    new TransitionFunctionArgument(2, " ", "<strong>"),
                    new TransitionFunctionValue(0, new [] {" "})
                },
                {
                    new TransitionFunctionArgument(2, "1", "<strong>"),
                    new TransitionFunctionValue(1, "")
                },
                {
                    new TransitionFunctionArgument(2, "_", null),
                    new TransitionFunctionValue(3, "_")
                },
                {
                    new TransitionFunctionArgument(2, "1", "1"),
                    new TransitionFunctionValue(2, new []{"1", "1"})
                },
                {
                    new TransitionFunctionArgument(2, "1", "K"),
                    new TransitionFunctionValue(2, new []{"1", "K"})
                },
                {
                    new TransitionFunctionArgument(2, "\\", null),
                    new AutomatonPartStarter(escapePartName, mainPartName, 2, "\\")
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
                    new TransitionFunctionArgument(3, "_", "_"),
                    new TransitionFunctionValue(0, @"<\strong>")
                },
                {
                    new TransitionFunctionArgument(3, "%", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(3, "K", "_"),
                    new AutomatonPartStarter(italicsPartName, mainPartName, 2,
                        new []{ "K", "<em>" })
                },
                {
                    new TransitionFunctionArgument(3, " ", "_"),
                    new TransitionFunctionValue(2, new[] {" ", "_"})
                },
                {
                    new TransitionFunctionArgument(3, "1", null),
                    new TransitionFunctionValue(2, "1")
                },
                {
                    new TransitionFunctionArgument(3, "\\", null),
                    new AutomatonPartStarter(escapePartName, mainPartName, 3, "\\")
                }

            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(3, transitionFunction);
        }

        #endregion

        #region EscapePart configuration
        private static AutomatonPart BuildMarkdownHandlerEscapePart()
        {
            var states = GetEscapePartStates();
            return new AutomatonPart(escapePartName, states);
        }

        private static List<IAutomatonState> GetEscapePartStates()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, "_", "\\"),
                    new TransitionFunctionValue(-1, "_")
                },
                {
                    new TransitionFunctionArgument(0, "1", null),
                    new TransitionFunctionValue(-1, "1")
                },
                {
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(-1, "K")
                },
                {
                    new TransitionFunctionArgument(0, "\\", "\\"),
                    new TransitionFunctionValue(-1, "")
                },
                {
                    new TransitionFunctionArgument(0, " ", null),
                    new TransitionFunctionValue(-1, " ")
                }
            };

            var transitionFunctions = new TransitionFunction(transitions);
            var state = new AutomatonState(0, transitionFunctions);
            return new List<IAutomatonState>() { state };
        }
        #endregion

        #region ItalicsPart configuration
        private static AutomatonPart BuildMarkdownHandlerItalicsPart()
        {
            var states = GetItalicsPartStates();
            return new AutomatonPart(italicsPartName, states);
        }

        private static List<IAutomatonState> GetItalicsPartStates()
        {
            return new List<IAutomatonState>() { GetItalicsPartState0(), GetItalicsPartState1(), GetItalicsPartState2() };
        }

        private static IAutomatonState GetItalicsPartState0()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(0, "1", "K"),
                    new TransitionFunctionValue(0, new []{"K", "1"})
                },
                {
                    new TransitionFunctionArgument(0, " ", null),
                    new TransitionFunctionValue(0, " ")
                },
                {
                    new TransitionFunctionArgument(0, "%", null),
                    new TransitionFunctionValue(0, "")
                },
                {
                    new TransitionFunctionArgument(0, "\\", "\\"),
                    new TransitionFunctionValue(0, "\\\\")
                },
                {
                    new TransitionFunctionArgument(0, "K", null),
                    new TransitionFunctionValue(0, "K")
                },
                {
                    new TransitionFunctionArgument(0, "_", null),
                    new TransitionFunctionValue(1, "_")
                },
                {
                    new TransitionFunctionArgument(0, "\\", null),
                    new AutomatonPartStarter(escapePartName, italicsPartName, 0, "\\")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(0, transitionFunction);
        }

        private static IAutomatonState GetItalicsPartState1()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(1, "1", "_"),
                    new TransitionFunctionValue(0, new []{"1", "_"})
                },
                {
                    new TransitionFunctionArgument(1, "K", null),
                    new TransitionFunctionValue(0, "K")
                },
                {
                    new TransitionFunctionArgument(1, "_", null),
                    new TransitionFunctionValue(2, "_")
                },
                {
                    new TransitionFunctionArgument(1, "K", "_"),
                    new TransitionFunctionValue(-1, new []{"K", @"<\em>"})
                },
                {
                    new TransitionFunctionArgument(1, " ", "_"),
                    new TransitionFunctionValue(-1, new []{" ", @"<\em>"})
                },
                {
                    new TransitionFunctionArgument(1, "%", "_"),
                    new TransitionFunctionValue(-1, @"<\em>")
                },
                {
                    new TransitionFunctionArgument(1, "\\", null),
                    new AutomatonPartStarter(escapePartName, italicsPartName, 1, "\\")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(1, transitionFunction);
        }

        private static IAutomatonState GetItalicsPartState2()
        {
            var transitions = new Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue>(comparer)
            {
                {
                    new TransitionFunctionArgument(2, "_", null),
                    new TransitionFunctionValue(1, "_")
                },
                {
                    new TransitionFunctionArgument(2, " ", null),
                    new TransitionFunctionValue(2, " ")
                },
                {
                    new TransitionFunctionArgument(2, "%", null),
                    new TransitionFunctionValue(2, "")
                },
                {
                    new TransitionFunctionArgument(2, "K", null),
                    new TransitionFunctionValue(2, "K")
                },
                {
                    new TransitionFunctionArgument(2, "\\", null),
                    new AutomatonPartStarter(escapePartName, italicsPartName, 2, "\\")
                }
            };

            var transitionFunction = new TransitionFunction(transitions);
            return new AutomatonState(2, transitionFunction);
        }
        #endregion
    }
}
