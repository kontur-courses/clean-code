using Markdown.Parsing;

namespace Markdown
{
	// Точка входа
	public class Md
	{
		private BlockParser _blockParser;
		private Rendering.HTMLRenderer _renderer;
		private Escaper _ecaper;

		public Md()
		{
			_blockParser = new BlockParser(new InlineParser());
			_renderer = new Rendering.HTMLRenderer();
			_ecaper = new Escaper();
		}

		// Преобразование текста в HTML по спецификации
		public string Render(string text)
		{
			// TODO: реализовать pipeline: экранирование, разбиение на абзацы, inline-разбор, рендер.
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}

			var escaped = _ecaper.Apply(text);
			var blocks = _blockParser.Parse(escaped);
			return _renderer.Render(blocks);
		}
	}
}