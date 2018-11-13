using System;

namespace Markdown
{
	class Program
	{
		static void Main()
		{
			var md = new Md();
			Console.WriteLine(md.Render("_hello __angry__ world_"));

		}
	}
}
