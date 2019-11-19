namespace Markdown
{
    public enum TypeEnum
    {
        TwoUnderScoreMd,
        OneUnderscoreMd,
        StrongHtml,
        EmHtml
    }

    public enum ClassEnum
    {
        Opener = 0,
        Closer = 1
    }
    
    public class TagTypeContainer
    {
        public int position;
        public ITag Tag;
        public TypeEnum TypeEnum => Tag.TypeEnum;
        public ClassEnum ClassEnum => Tag.ClassEnum;
        
        
        public TagTypeContainer(ITag tag, bool TagClass, int position)
        {
            switch (TagClass)
            {
                case true:
                    tag.ClassEnum = ClassEnum.Closer;
                    break;
                case false:
                    tag.ClassEnum = ClassEnum.Opener;
                    break;
            }
            this.position = position;
            this.Tag = tag;
        }
        

        public static bool operator ==(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TypeEnum == t2.Tag.TypeEnum && t1.Tag.ClassEnum == t2.Tag.ClassEnum;
        }
        
        public static bool operator !=(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TypeEnum != t2.Tag.TypeEnum && t1.Tag.ClassEnum != t2.Tag.ClassEnum;
        }
    }
}
            