using System;

namespace Markdown
{
	public class Md
	{
		public string Render(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			var converter = new MdConverter();
			return converter.ConvertToHtml(text);
		}
	}
}