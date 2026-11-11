namespace Markdown.Parsing
{
	// Контекст для inline-правил
	// Хранит вход, маску экранирования, границы абзаца
	public class RuleContext
	{
		public string Text { get; }
		public int Start { get; }
		public int End { get; }

		public RuleContext(string text, int start, int end)
		{
			Text = text;
			Start = start;
			End = end;
		}

		public bool IsMasked (int index)
		{
			return (index > 0 && index < Text.Length && Text[index - 1] == Escaper.Mask);
		}
	}
}
