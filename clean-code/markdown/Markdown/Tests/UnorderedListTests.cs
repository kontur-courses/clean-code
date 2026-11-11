using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class UnorderedListTests
	{
		private Md _md = new();

		[Test]
		public void SingleItemList()
		{
			_md.Render("- один").Should().Be("<ul><li>один</li></ul>");
		}

		[Test]
		public void MultipleItemsList()
		{
			_md.Render("- раз\n- два\n- три").Should().Be("<ul><li>раз</li><li>два</li><li>три</li></ul>");
		}

		[Test]
		public void ListItemWithFormatting()
		{
			_md.Render("- _курсив_").Should().Be("<ul><li><em>курсив</em></li></ul>");
		}

		[Test]
		public void ListDoNotFollowedByParagraph()
		{
			_md.Render("- раз\n\nтекст").Should().Be("<ul><li>раз</li></ul><p>текст</p>");
		}
	}
}
