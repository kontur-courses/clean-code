using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{

	public enum TagType
	{
		Opening,
		Closing,
		Undefined
	}

	public static class Initializer
	{
		public static TokenDescription[] GetTokenDescriptions()
		{
			return new []
			{
				new TokenDescription("Text", null, null, null),
				new TokenDescription("Italics", "_", "<em>", null),
				new TokenDescription("Strong", "__", "<strong>", null)
			};
		}
	}
}
