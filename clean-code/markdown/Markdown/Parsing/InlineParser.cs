using Markdown.Models;

namespace Markdown.Parsing
{
	// Конвейер inline-правил. Хранит упорядоченный список IInlineRule
	// и умеет разбирать абзац целиком в список узлов.
	public class InlineParser
	{
		private IReadOnlyList<IInlineRule> _rules;

		public InlineParser()
		{
			// TODO: зарегистрировать правила в правильном порядке:
			// 1) HeadingRule (только в начале абзаца),
			// 2) StrongRule (__ раньше _, чтобы ___ корректно разрешалось),
			// 3) EmphasisRule (_ после __).
			throw new NotImplementedException();
		}

		// Разбирает один абзац в дерево INode.
		public IReadOnlyList<INode> Parse(string abzats)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
