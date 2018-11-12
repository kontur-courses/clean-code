namespace Markdown
{
	public class Md
	{
		public string Render(string text)
		{
			var stream = new TextStream(text);
			var converter = new MdConverter();
			return converter.ConvertToHtml(stream);
		}
	}
}