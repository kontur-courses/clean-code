using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tag
{
	class SharpTag : ITag
	{
		public string Symbol { get; set; } = "#";
		public int Length { get; set; } = 1;
		public int OpenIndex { get; set; }
		public int CloseIndex { get; set; }
		public string HtmlOpen { get; set; } = "<h1>";
		public string HtmlClose { get; set; } = "</h1>";
	}
}
