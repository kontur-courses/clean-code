namespace Markdown
{
    class Md
    {
        public string Render(string markDownInput)
        {
            var converter = new MdToHTMLConverter();
            var result = converter.Convert(markDownInput);
            return result;
        }
    }
}