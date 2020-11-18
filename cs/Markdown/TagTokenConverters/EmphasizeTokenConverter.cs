namespace Markdown
{
    public class EmphasizeTokenConverter : TagTokenConverter
    {
        public EmphasizeTokenConverter()
        {
            OpenTag = "<em>";
            CloseTag = "</em>";
        }
    }
}