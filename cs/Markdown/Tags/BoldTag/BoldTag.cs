namespace Markdown.Tags.BoldTag
{
    public class BoldTag : Tag
    {
        public const int TagLength = 2;
        
        public BoldTag(string value, int index) : base(value, index, TagLength)
        {
            
        }
    }
    
}