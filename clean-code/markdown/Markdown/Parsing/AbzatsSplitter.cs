namespace Markdown.Parsing
{
	public class AbzatsSplitter
	{
		// Делит исходный текст на абзацы. Абзац — единица обработки правил
		// Два и более \n это новый абзац, один - просто новая строка
		public IReadOnlyList<string> Split(string text)
		{
			var result = new List<string>();
			int start = 0;
			int i = 0;

			while (i < text.Length)
			{
				if (text[i] != '\n')
				{
					i++;
					continue;
				}
				// Ищем второй перенос учитывая, что они могут быть разделены
				int j = i + 1;
				while (j < text.Length && (text[j] == ' ' || text[j] == '\r' || text[j] == '\t'))
				{
					j++;
				}
				if (j < text.Length && text[j] == '\n')
				{
					AddIfNonEmpty(result, text, start, i);
					// Пропускаем всю серию пустых строк (могут быть \n \n \n ...).
					while (j < text.Length)
					{
						if (text[j] == '\n')
						{
							j++;
							// после '\n' тоже могут быть пробелы до следующего '\n'
							while (j < text.Length && (text[j] == ' ' || text[j] == '\t' || text[j] == '\r'))
							{
								j++;
							}
						}
						else if (text[j] == ' ' || text[j] == '\t' || text[j] == '\r')
						{
							j++;
						}
						else
						{
							break;
						}
					}

					start = j;
					i = j;
					continue;
				}
				// Если был найден только один перенос, то это не граница, идём дальше.
				i++;
			}
			AddIfNonEmpty(result, text, start, text.Length);
			return result;
		}
			
		private void AddIfNonEmpty(List<string> result, string text, int start, int end)
		{
			if (end > start)
			{
				result.Add(text.Substring(start, end - start));
			}
		}
	}
}
