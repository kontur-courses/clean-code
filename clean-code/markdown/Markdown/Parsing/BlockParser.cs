using Markdown.Models;

namespace Markdown.Parsing
{
	// Разбирает текст на блоки:
	// строка, начинающаяся с "#" - HeadingNode
	// строка, начинающаяся с "-" - элемент списка (собираются в UnorderedListNode).
	// Внутри абзаца одиночные '\n' пробел при рендере.
	// иначе ParagraphNode.
	public class BlockParser
	{
		private InlineParser _inline;
		private AbzatsSplitter _splitter = new();

		public BlockParser(InlineParser inline)
		{
			_inline = inline;
		}

		public IReadOnlyList<INode> Parse(string text)
		{
			var abzatsList = _splitter.Split(text);
			var blocks = new List<INode>();

			foreach (var abzats in abzatsList)
			{
				blocks.Add(AbzatsParser(abzats));
			}

			return blocks;
		}

		private INode AbzatsParser(string abzats)
		{
			if (abzats.StartsWith("# "))
			{
				string content = abzats.Substring(2).Replace('\n', ' ');
				return new HeadingNode(_inline.Parse(content));
			}

			if (IsUnorderedList(abzats))
			{
				var items = new List<INode>();
				foreach (var line in abzats.Split('\n'))
				{
					string content = line.Substring(2);
					items.Add(new ListItemNode(_inline.Parse(content)));
				}
				return new UnorderedListNode(items);
			}

			string paragraph = abzats.Replace('\n', ' ');
			return new ParagraphNode(_inline.Parse(paragraph));
		}

		private bool IsUnorderedList(string abzats)
		{
			if (!abzats.StartsWith("- ")) return false;

			int i = 0;
			while (i < abzats.Length)
			{
				int lineEnd = abzats.IndexOf('\n', i);
				if (lineEnd < 0) lineEnd = abzats.Length;
				int len = lineEnd - i;
				if (len == 0) return false;
				if (abzats[i] != '-' || i + 1 >= lineEnd || abzats[i + 1] != ' ') return false;
				i = lineEnd + 1;
			}
			return true;
		}
	}
}
