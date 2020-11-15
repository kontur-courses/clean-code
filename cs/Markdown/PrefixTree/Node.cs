using System.Collections.Generic;

namespace Markdown
{
    public class Node
    {
        public readonly string Value;
        public readonly bool IsFinishNode;
        public Dictionary<char, Node> Connections { get; private set; }
        public int Depth { get; }

        public Node(string value, int depth, bool isFinish)
        {
            Value = value;
            Depth = depth;
            IsFinishNode = isFinish;
            Connections = new Dictionary<char, Node>();
        }

        public void Connect(char symbol, Node node)
        {
            Connections[symbol] = node;
        }
    }
}