using Markdown.Models;
using System.Text;

namespace Markdown.Parsing
{
	//  Разбирает абзац в дерево узлов.
	public class InlineParser
	{
		private StrongRule _strong = new();
		private EmphasisRule _emphasis = new();

		// Разбирает один абзац в дерево INode.
		public IReadOnlyList<INode> Parse(string text)
		{
			var context = new RuleContext(text, 0, text.Length);
			return ParseRange(context, 0, text.Length);
		}

		// Разбирает текст в указанном диапазоне
		// Сначала делает __, затем _
		private IReadOnlyList<INode> ParseRange(RuleContext context, int start, int end)
		{
			var result = new List<INode>();
			int pos = start;
			int textStart = start;

			while(pos < end)
			{	
				// Пытаемся распознать __
				if (_strong.TryMatch(context, pos, end, out int strongEnd, out int strongLen))
				{
					FlushText(result, context, textStart, pos);
					var children = ParseRange(context, pos + strongLen, strongEnd);
					result.Add(new StrongNode(children));
					pos = strongLen + strongEnd;
					textStart = pos;
					continue;
				}
				// Пытаемся распознать _
				if (_emphasis.TryMatch(context, pos, end, out int emEnd, out int emLen))
				{
					FlushText(result, context, textStart, pos);
					var children = new INode[] { new TextNode(Unmask(context.Text, pos + emLen, emEnd)) };
					result.Add(new EmNode(children));
					pos = emLen + emEnd;
					textStart = pos;
					continue;
				}
				pos++;
			}
			FlushText(result, context, textStart, pos);
			return result;
		}

		private void FlushText(List<INode> result, RuleContext context, int start, int end)
		{
			if (end > start)
			{
				result.Add(new TextNode(Unmask(context.Text, start, end)));
			}
		}

		// Убираем маски экранирования из готового текста
		private string Unmask(string text, int start, int end)
		{
			var sb = new StringBuilder(end - start);
			int i = start;

			while (i < end)
			{
				if (text[i] == Escaper.Mask && i + 1 < end)
				{
					sb.Append(text[i + 1]);
					i += 2;
				}
				else
				{
					sb.Append(text[i]);
					i++;
				}
			}
			return sb.ToString();
		}
	}
}
