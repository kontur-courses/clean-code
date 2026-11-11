using Markdown.Models;

namespace Markdown.Parsing
{
	public class EmphasisRule
	{
		// Правило для '_'.
		// Учитывает:
		//  - запрет выделения внутри цифр (цифры_12_3),
		//  - запрет через пробел с обеих сторон,
		//  - части слова (нач_але, сер_еди_не, кон_це) — работают,
		//  - через слова (ра_зных сл_овах) — нет,
		//  - внутри __ … __ работает, внутри _ … _ — нет.
		public bool TryMatch(RuleContext context, int pos, int end,
			out int emEnd, out int emLen)
		{
			emEnd = -1;
			emLen = 1;

			if (pos + 1 >= end) return false;
			if (context.Text[pos] != '_') return false;
			if (context.IsMasked(pos)) return false;
			if (pos + 1 < end && context.Text[pos + 1] == '_' && !context.IsMasked(pos + 1)) return false;
			if (pos > 0 && context.Text[pos - 1] == '_' && !context.IsMasked(pos - 1)) return false;
			if (char.IsWhiteSpace(context.Text[pos + 1])) return false;

			bool openInsideWord = pos > 0 && char.IsLetterOrDigit(context.Text[pos - 1]);
			int i = pos + 1;
			int doubleUnderscoresCount = 0;
			while (i < end)
			{
				if (openInsideWord && char.IsWhiteSpace(context.Text[i])) return false;

				if (context.Text[i] == '_' && !context.IsMasked(i))
				{
					// Проверяем, не является ли это подчеркивание частью двойного __
					bool prevIsUnderscore = i - 1 >= pos && context.Text[i - 1] == '_' && !context.IsMasked(i - 1);
					bool nextIsUnderscore = i + 1 < end && context.Text[i + 1] == '_' && !context.IsMasked(i + 1);

					// Если это часть двойного __, и при этом это не трёшка
					if (prevIsUnderscore || nextIsUnderscore)
					{
						doubleUnderscoresCount++;
						// Если это тройное ___ в конце, то последнее подчеркивание 
						// (где nextIsUnderscore == false, а prevIsUnderscore == true) 
						// должно сработать как закрывающее.
						// Если это просто __, мы его перепрыгиваем.
						if (nextIsUnderscore)
						{
							i += 2;
							continue;
						}
					}

					// Проверяем, что перед закрывающей не пробел
					if (i > pos + 1 && !char.IsWhiteSpace(context.Text[i - 1]))
					{
						bool innerLeftDigit = char.IsDigit(context.Text[pos + 1]);
						bool innerRightDigit = char.IsDigit(context.Text[i - 1]);
						if (innerLeftDigit && innerRightDigit) return false;
						// Если doubleUnderscoresCount нечётный, значит внути em
						// есть __ без пары, тогда появляется пересечение.
						if (doubleUnderscoresCount % 2 != 0) return false;

						emEnd = i;
						return true;
					}
				}
				i++;
			}
			return false;
		}
	}
}
