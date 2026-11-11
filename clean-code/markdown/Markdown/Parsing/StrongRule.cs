using Markdown.Models;

namespace Markdown.Parsing
{
	// Правило для '__'.
	// Взаимодействует с EmphasisRule: внутри strong одинарные работают,
	// при пересечении ___…___ ни один не считается выделением.
	// Пустое ____ (пустая строка) — литералы.
	public class StrongRule
	{
		public bool TryMatch(RuleContext context, int pos, int end,
			out int strongEnd, out int openLen)
		{
			strongEnd = -1;
			openLen = 2;

			if (pos + 1 >= end) return false;
			if (context.Text[pos] != '_' || context.Text[pos + 1] != '_') return false;
			if (context.IsMasked(pos) || context.IsMasked(pos + 1)) return false;
			// После открывающей не должно быть пробела
			if (pos + 2 >= end) return false;
			if (char.IsWhiteSpace(context.Text[pos + 2])) return false;
			// Защита от пустого выделения
			// Внутри не должно быть пустой строки, ____ — частный случай,
			// если сразу после открытия идёт '__', это литералы.
			if (pos + 3 < end && context.Text[pos + 2] == '_' && context.Text[pos + 3] == '_') return false;

			int oneUnderscoresCount = 0;
			int i = pos + 2;
			while (i + 1 < end)
			{
				if (context.Text[i] == '_' && context.Text[i + 1] != '_') 
				{
					oneUnderscoresCount++;
				}
				if (i + 2 < end && context.Text[i + 2] == '_' && !context.IsMasked(i + 2))
				{
					i++;
					continue;
				}
				if (context.Text[i] == '_' && context.Text[i + 1] == '_'
					&& !context.IsMasked(i) && !context.IsMasked(i + 1))
				{
					if (i > pos + 2 && !char.IsWhiteSpace(context.Text[i - 1]))
					{
						// Нечётное количество _ внутри пары __, значит есть пересечение
						if (oneUnderscoresCount % 2 == 0)
						{
							strongEnd = i;
							return true;
						}
						
					}
				}
				i++;
			}
			return false;
		}
	}
}
