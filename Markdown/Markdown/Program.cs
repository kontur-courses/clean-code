using System;

namespace Markdown
{
	class Program
	{
		static void Main()
		{
			var md = new Md();
			Console.WriteLine(md.Render("__aa _bb_ _cc_ dd__"));
		}
	}
}