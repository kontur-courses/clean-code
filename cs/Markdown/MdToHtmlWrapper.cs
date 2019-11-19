using System.Collections.Generic;

namespace Markdown
{
    public interface IWrapper
    {
        string WrapWithTag(ITag tag, Token token);
    }

    public class MdToHtmlWrapper : IWrapper
    {
        private Dictionary<string, string> tagMap;

        public MdToHtmlWrapper(Dictionary<string, string> map)
        {
            tagMap = map;
        }

        public static Dictionary<string, string> getMdToHtmlDefaultMap()
        {
            var dict = new Dictionary<string, string>
            {
                {new TwoUnderscoreTag().StringTag, new StrongTag().StringTag},
                {new OneUnderscoreTag().StringTag, new EmTag().StringTag}
            };
            return dict;
        }

        public string WrapWithTag(ITag tag, Token token)
        {
            return $"{tagMap[tag.StringTag]}{token.Value}"+tagMap[tag.StringTag].Insert(1,"/");
        }

        public static Token GetToken(TagTypeContainer closerTag, TagTypeContainer openerTag, string result)
        {
            var end = closerTag.position + closerTag.Tag.Length - 1;
            var stringValueStart = openerTag.position + openerTag.Tag.Length;
            var token = new Token(result.ToString().Substring(stringValueStart, closerTag.position - stringValueStart),
                openerTag.position, end);
            return token;
        }

    }
}