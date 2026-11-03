using Markdown.Models;

namespace Markdown.Parsing
{
	// Одно правило инлайн-разбора. Порядок правил в конвейере задаётся
	// в InlineParser (важно: __ должен обрабатываться раньше _, # — на уровне абзаца).
	public interface IInlineRule
	{
		// Пытается распознать правило начиная с позиции start
		// Возвращает узел и новую позицию, либо null, если правило не сработало.
		bool TryParse(string text, int start, out INode? node, out int nextIndex);
	}
}
