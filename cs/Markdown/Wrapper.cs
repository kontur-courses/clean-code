using System.Collections.Generic;

namespace Markdown
{
    public abstract class Wrapper
    {
        protected Dictionary<string, string> tagMap;

        public Wrapper(Dictionary<string, string> map)
        {
            tagMap = map;
        }

        public abstract void SetDefaultMap();

        public abstract string WrapWithTag(ITag tag, Token token);
    }

    public class MdToHtmlWrapper : Wrapper
    {
        public MdToHtmlWrapper(Dictionary<string, string> map) : base(map)
        {
        }

        public static Dictionary<string, string> GetDefaultMap()
        {
            var dict = new Dictionary<string, string>
            {
                {MDTag.GetOneUnderscoreTag().StringTag, new HTMLTag("<em>", TagTypeEnum.StrongHtml).StringTag},
                {MDTag.GetTwoUnderscoreTag().StringTag, new HTMLTag("<strong>", TagTypeEnum.EmHtml).StringTag}
            };
            return dict;
        }

        public override void SetDefaultMap()
        {
            tagMap = GetDefaultMap();
        }

        public override string WrapWithTag(ITag tag, Token token)
        {
            return $"{tagMap[tag.StringTag]}{token.Value}" + tagMap[tag.StringTag].Insert(1, "/");
        }
    }
}