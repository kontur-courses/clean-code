using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal enum TokenType
    {
        Word,
        Space,
        Underscore,
        DoubleUnderscore,
        Newline,
        Backslash,
        HeaderStart
    }

    internal class Token
    {
        public TokenType Type { get; set; }
        public bool HasBody { get; set; }
        public string Text { get; set; }
    }

    internal class StateToStatePath
    {
        public StateNode Destination { get; set; }
        public Func<char, bool> Match { get; set; }

        public StateToStatePath(StateNode destination, Func<char, bool> match)
        {
            Destination = destination;
            Match = match;
        }
    }

    internal class StateNode
    {
        public List<StateToStatePath> Pathes { get; } = new List<StateToStatePath>();
        public Func<string, Token> GetResult { get; set; }

        public StateNode(Func<string, Token> getResult)
        {
            GetResult = getResult;
        }
    }

    internal class StateGraph
    {
        public List<StateNode> Nodes { get; } = new List<StateNode>();
        public StateNode CurrentState { get; set; }

        public StateGraph()
        {
            var whitespace = ' ';
            var underscore = '_';
            var backslash = '\\';
            var newline = '\n';

            var wordNode = new StateNode(buffer => new Token() { Type = TokenType.Word, Text = buffer });
            var spaceNode = new StateNode(buffer => new Token() { Type = TokenType.Space, Text = buffer });
            var underscoreNode = new StateNode(buffer => new Token { Type = TokenType.Underscore, Text = buffer, HasBody = true });
            var newlineNode = new StateNode(buffer => new Token { Type = TokenType.Newline, Text = buffer });
            var backslashNode = new StateNode(buffer => new Token { Type = TokenType.Backslash, Text = buffer });
            var headerStartNode = new StateNode(buffer => new Token { Type = TokenType.HeaderStart, Text = buffer, HasBody = true });

            var toWordNodePath = new StateToStatePath(wordNode, c => char.IsLetterOrDigit(c));
            var toSpaceNodePath = new StateToStatePath(spaceNode, c => c == whitespace);
            var toUnderscoreNodePath = new StateToStatePath(underscoreNode, c => c == underscore);
            var toNewlineNodePath = new StateToStatePath(newlineNode, c => c == newline);
            var toBackslashNodePath = new StateToStatePath(backslashNode, c => c == backslash);

            wordNode.Pathes.AddRange(new[]
            {
                toWordNodePath,
                toSpaceNodePath,
                toUnderscoreNodePath,
                toNewlineNodePath,
                toBackslashNodePath
            });

            spaceNode.Pathes.AddRange(new[]
            {
                toWordNodePath,
                toUnderscoreNodePath,
                toBackslashNodePath
            });

            underscoreNode.Pathes.AddRange(new[]
            {
                toWordNodePath,
                toBackslashNodePath,
                toSpaceNodePath
            });

            backslashNode.Pathes.AddRange(new[]
            {
                toWordNodePath,
                toSpaceNodePath,
                toNewlineNodePath,
                toUnderscoreNodePath
            });

            CurrentState = wordNode;
        }
    }
}