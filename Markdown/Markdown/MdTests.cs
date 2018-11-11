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
		public void WithoutSpecialCharacters_ShouldRenderWithoutChanges()
		{
			var text = "hello world";
			var actual = md.Render(text);

			Assert.AreEqual(actual, "hello world");
		}


		[Test]
		public void UnderLineCharacter_ShouldRenderToHtmlTag()
		{
			var text = "hello _world_";
			var actual = md.Render(text);

			Assert.AreEqual(actual, "hello <em>world</em>");
		}

		[Test]
		public void ScreenUnderLineCharacter_ShouldRenderWithoutChanges()
		{
			var text = @"hello \_world\_";
			var actual = md.Render(text);

			Assert.AreEqual(actual, "hello _world_");
		}
	}
}
