namespace Markdown
{
	public class Md
	{
		public string Render(string text)
		{
			var converter = new MdConverter();
			return converter.ConvertToHtml(text);
		}
	}
}