namespace Markdown
{
	// Точка входа
	public class Md
	{
		private Parsing.InlineParser _inlineParser;
		private Rendering.HTMLRenderer _renderer;

		public Md()
		{
			_inlineParser = new Parsing.InlineParser();
			_renderer = new Rendering.HTMLRenderer();
		}

		// Преобразование текста в HTML по спецификации
		public string Render(string text)
		{
			// TODO: реализовать pipeline: экранирование, разбиаение на абзацы, inline-разбор, рендер.
			throw new NotImplementedException();
		}
	}
}