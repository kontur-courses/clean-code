using Markdown.Models;

namespace Markdown.Parsing
{
	public class HeadingRule : IInlineRule
	{
		// Правило для '# ' в начале абзаца.
		// Тело заголовка разбирается теми же inline-правилами (_, __).
		public bool TryParse(string text, int start, out INode? node, out int nextIndex)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
