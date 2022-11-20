namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            if (string.IsNullOrWhiteSpace((text)))
                return text;

            var converter = new Converter.Converter();
            var resultInHtml = converter.Convert(text);
            return resultInHtml;

        }
    }
}