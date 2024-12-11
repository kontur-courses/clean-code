using Markdown.Tokenizer.Tags;
using Markdown.TreeBuilder.Nodes;

namespace Markdown.TreeBuilder;

public class NodeFactory : INodeFactory
{
	public NodeAction? CreateNode(Token token)
	{
		return token switch
		{
			{ TagStatus: TagStatus.Broken } => null,
			ItalicTag { TagStatus: TagStatus.Open } => new NodeAction.OpenNode(new ItalicNode()),
			ItalicTag { TagStatus: TagStatus.Closed } => new NodeAction.CloseNode(),
			BoldTag { TagStatus: TagStatus.Open } => new NodeAction.OpenNode(new BoldNode()),
			BoldTag { TagStatus: TagStatus.Closed } => new NodeAction.CloseNode(),
			SlashToken { TagStatus: TagStatus.Escaped } => new NodeAction.SkipNode(),
			HeaderTag => new NodeAction.OpenNode(new HeaderNode()),
			NewLineToken => new NodeAction.SkipNode(),
			_ => null
		};
	}
}