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
        Opener = 1,
        Closer = 2,
        Empty = 0
    }
    
    public class TagTypeContainer
    {
        public TagTypeEnum TagType;
        public TagClassEnum TagClass;
        public int position;
        
        public TagTypeContainer(string tag, int TagClass, int position)
        {
            switch (tag)
            {
                case "__":
                    this.TagType = TagTypeEnum.TwoUnderScore;
                    break;
                case "_":
                    this.TagType = TagTypeEnum.OneUnderscore;
                    break;
                case "":
                    this.TagType = TagTypeEnum.Nothing;
                    break;
            }
            this.TagClass = (TagClassEnum) TagClass;
            this.position = position;
        }
        
        public TagTypeContainer(TagTypeEnum tag, TagClassEnum TagClass, int pos)
        {
            this.TagType = tag;
            this.TagClass = TagClass;
            position = pos;
        }
        
  
        
        public static bool operator ==(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.TagType == t2.TagType && t1.TagClass == t2.TagClass;
        }
        
        public static bool operator !=(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.TagType != t2.TagType && t1.TagClass != t2.TagClass;
        }
    }
}
            