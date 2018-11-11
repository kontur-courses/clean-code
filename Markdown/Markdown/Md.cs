using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
	public class Md
	{
		public string Render(string text)
		{
			var converter = new MdConverter(text);
			return converter.ConvertToHtml();
		}
	}
}