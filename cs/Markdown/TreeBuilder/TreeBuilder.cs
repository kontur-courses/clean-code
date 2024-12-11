using Markdown.Tokenizer.Tags;
using Markdown.TreeBuilder.Nodes;

namespace Markdown.TreeBuilder;

public class TreeBuilder(INodeFactory nodeFactory)
{
	public Node Build(List<Token> tokens)
	{
		Node mainNode = new MainNode();
		var currentNode = mainNode;

		foreach (var token in tokens)
		{
			var nodeAction = nodeFactory.CreateNode(token);

			if (nodeAction == null)
			{
				currentNode.Children.Add(new TextNode { Value = token.Value });
				continue;
			}

			switch (nodeAction)
			{
				case NodeAction.OpenNode openNode:
					currentNode.Children.Add(openNode.Node);
					openNode.Node.Parent = currentNode;
					currentNode = openNode.Node;
					break;

				case NodeAction.CloseNode:
					currentNode = currentNode.Parent ?? currentNode;
					break;

				case NodeAction.SkipNode:
					break;
			}
		}

		return mainNode;
	}
}