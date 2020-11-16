using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class PrefixTree
    {
        public Node Root = new Node("", 0, false);

        public PrefixTree(List<string> words)
        {
            words = words.OrderBy(word => word.Length).ToList();
            foreach (var word in words)
            {
                var currentNode = Root;
                for (var i = 0; i < word.Length; i++)
                {
                    if (!currentNode.Connections.Keys.Contains(word[i]))
                    {
                        var nextNodeIsFinish = i == word.Length - 1;
                        var nextNode = new Node(currentNode.Value + word[i],
                            Root.Depth + 1, nextNodeIsFinish);
                        currentNode.Connect(word[i], nextNode);
                    }

                    currentNode = currentNode.Connections[word[i]];
                }
            }
        }
    }
}