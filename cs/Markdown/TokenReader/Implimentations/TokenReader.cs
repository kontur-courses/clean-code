using System.Collections.Generic;
using System.Text;
using Markdown.Properties;

namespace Markdown
{
    public class TokenReader
    {
        private readonly FiniteDerminationAutomaton automaton;
        private readonly Dictionary<State, Tag> terminateStates;
        private readonly TokenFlagLayouter tokenFlagLayouter;


        public TokenReader(Dictionary<string, Tag> tags)
        {
            terminateStates = new Dictionary<State, Tag>();
            automaton = new FiniteDerminationAutomaton();
            foreach (var tag in tags)
                terminateStates[automaton.AddNewTag(tag.Key)] = tag.Value;

            tokenFlagLayouter = new TokenFlagLayouter();
            tokenFlagLayouter.AddFlagDependency(ch => char.IsWhiteSpace(ch), token => token.IsWhiteSpace = true);
            tokenFlagLayouter.AddFlagDependency(ch => char.IsDigit(ch), token => token.HasNumber = true);
        }


        public LinkedList<Token> ReadTokens(string source)
        {
            var tokens = new LinkedList<Token>();
            var startState = automaton.GetInitialState();
            var stringBuilder = new StringBuilder();

            var token = new Token();
            var currentState = startState;
            foreach (var symbol in source)
            {
                var nextState = currentState.GetNextState(symbol);

                if (nextState == startState)
                {
                    token.Value = stringBuilder.ToString();
                    if (terminateStates.ContainsKey(currentState))
                        token.PosibleTag = terminateStates[currentState];
                    tokens.AddLast(token);

                    token = new Token();
                    stringBuilder.Clear();
                    currentState = startState.GetNextState(symbol);
                    tokenFlagLayouter.LyaoutTags(symbol, token);
                    if (symbol != '\\')
                        stringBuilder.Append(symbol);
                }
                else
                {
                    currentState = nextState;
                    stringBuilder.Append(symbol);
                    tokenFlagLayouter.LyaoutTags(symbol, token);
                }
            }

            token.Value = stringBuilder.ToString();
            if (terminateStates.ContainsKey(currentState))
                token.PosibleTag = terminateStates[currentState];
            tokens.AddLast(token);

            return tokens;
        }
    }
}