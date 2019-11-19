namespace Markdown
{
    public interface ITag
    {
        string StringTag { get; }
        int Length { get; }
        TypeEnum TypeEnum { get; }
        ClassEnum ClassEnum { get; set; }
    }

    public class TwoUnderscoreTag : ITag
    {
        public string StringTag => "__";
        public int Length { get; }
        public TypeEnum TypeEnum { get; }
        public ClassEnum ClassEnum { get; set; }

        public TwoUnderscoreTag()
        {
            TypeEnum = TypeEnum.TwoUnderScoreMd;
            this.Length = StringTag.Length;
        }
    }
    
    public class StrongTag : ITag
    {
        public string StringTag => "<strong>";
        public int Length { get; }
        public TypeEnum TypeEnum { get; }
        public ClassEnum ClassEnum { get; set; }

        public StrongTag()
        {
            TypeEnum = TypeEnum.StrongHtml;
            this.Length = StringTag.Length;
        }
    }
    
    public class EmTag : ITag
    {
        public string StringTag => "<em>";
        public int Length { get; }
        public TypeEnum TypeEnum { get; }
        public ClassEnum ClassEnum { get; set; }

        public EmTag()
        {
            TypeEnum = TypeEnum.EmHtml;
            this.Length = StringTag.Length;
        }
    }

    public class OneUnderscoreTag : ITag
    {
        public string StringTag => "_";
        public int Length { get; }
        public TypeEnum TypeEnum { get; }
        public ClassEnum ClassEnum { get; set; }

        public OneUnderscoreTag()
        {
            TypeEnum = TypeEnum.OneUnderscoreMd;
            this.Length = StringTag.Length;
        }
    }
}