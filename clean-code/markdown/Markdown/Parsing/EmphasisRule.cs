using Markdown.Models;

namespace Markdown.Parsing
{
	public class EmphasisRule : IInlineRule
	{
		// Правило для '_'.
		// Учитывает:
		//  - запрет выделения внутри цифр (цифры_12_3),
		//  - запрет через пробел с обеих сторон,
		//  - части слова (нач_але, сер_еди_не, кон_це) — работают,
		//  - через слова (ра_зных сл_овах) — нет,
		//  - внутри __ … __ работает, внутри _ … _ — нет.
		public bool TryParse(string text, int start, out INode? node, out int nextIndex)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
