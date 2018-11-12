using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	class Program
	{
		static void Main()
		{
			var md = new Md();
			Console.WriteLine(md.Render("__hello world__"));

		}
	}
}
