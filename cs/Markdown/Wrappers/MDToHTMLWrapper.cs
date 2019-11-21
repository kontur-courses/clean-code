using System.Collections.Generic;
using Markdown.DTOs;
using Markdown.Tags;

namespace Markdown.Wrappers
{
    public class MDToHTMLWrapper : Wrapper
    {
        public MDToHTMLWrapper(Dictionary<string, string> map) : base(map)
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