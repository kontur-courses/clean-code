namespace Markdown.Types
{
    public class Token
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public  string Value { get; set; }
        public TypeToken TypeToken { get; set; }
    }
}