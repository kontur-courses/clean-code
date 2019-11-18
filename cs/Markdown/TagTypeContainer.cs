namespace Markdown
{
    public enum TagTypeEnum
    {
        TwoUnderScore,
        OneUnderscore,
        Nothing
    }

    public enum TagClassEnum
    {
        Opener = 0,
        Closer = 1
    }
    
    public class TagTypeContainer
    {
        public int position;
        public ITag Tag;
        
        public TagTypeContainer(ITag tag, bool TagClass, int position)
        {
            switch (TagClass)
            {
                case true:
                    tag.TagClass = TagClassEnum.Closer;
                    break;
                case false:
                    tag.TagClass = TagClassEnum.Opener;
                    break;
            }
            this.position = position;
            this.Tag = tag;
        }
        

        public static bool operator ==(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TagType == t2.Tag.TagType && t1.Tag.TagClass == t2.Tag.TagClass;
        }
        
        public static bool operator !=(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TagType != t2.Tag.TagType && t1.Tag.TagClass != t2.Tag.TagClass;
        }
    }
}
            