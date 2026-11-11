using System.Text;

namespace Markdown.Parsing
{
	public class Escaper
	{
		// Обрабатывает экранирование '\':
		// - '\_' → '_' (символ становится литералом),
		// - '\\' → '\',
		// - одиночный '\' перед обычным символом остаётся '\'.
		// Выполняется ДО разбора разметки.
		public const char Mask = '\u0001';

		public string Apply(string text)
		{
			// Возвращает "маскированную" строку, где экранированные символы
			// уже заменены на плейсхолдеры/литералы
			var sb = new StringBuilder(text.Length);
			int i = 0;
			while (i < text.Length)
			{
				char c = text[i];
				if (c == '\\' &&  i + 1 < text.Length)
				{
					char next = text[i + 1];
                    if (next == '_' || next == '\\' || next == '#' || next == '-' || next == Mask)
                    {
						sb.Append(Mask);
						sb.Append(next);
						i += 2;
						continue;
                    }
                }
				sb.Append(c);
				i++;
			}
			return sb.ToString();
		}
	}
}
