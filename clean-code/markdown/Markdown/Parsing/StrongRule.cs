using Markdown.Models;

namespace Markdown.Parsing
{
	// Правило для '__'.
	// Взаимодействует с EmphasisRule: внутри strong одинарные работают,
	// при пересечении ___…___ ни один не считается выделением.
	// Пустое ____ (пустая строка) — литералы.
	public class StrongRule : IInlineRule
	{
		public bool TryParse(string text, int start, out INode? node, out int nextIndex)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
