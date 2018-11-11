using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	class MdTests
	{
		private Md md;

		[SetUp]
		public void SetUp()
		{
			md = new Md();
		}


		[Test]
		public void Render_LineWithUnderLineTag_ShouldReturnLineWithHtmlTag()
		{
			var text = "hello _world_";
			var actual = md.Render(text);

			Assert.AreEqual(actual, "hello <em>world</em>");
		}
	}
}
