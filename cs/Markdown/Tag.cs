using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public class Tag
    {
        public static List<Func<Tag, bool, string, bool>> DefaultTextContentRules = new List<Func<Tag, bool, string, bool>>
        {
            (tag, isPartial, text) =>
                !text
                    .Any(e => e == '\n'),
            (tag, isPartial, text) =>
                !text
                    .All(char.IsWhiteSpace),
        };
        public static List<Func<Token, List<Token>, bool>> DefaultTagInteractionRules = new List<Func<Token, List<Token>, bool>>
        {

        };

        public string OpenMark;
        public string CloseMark;
        public string HtmlTag;
        public List<Func<Tag, bool, string, bool>> TextContentRules;
        public List<Func<Token, List<Token>, bool>> TagInteractionRules;

        public Tag(string openMark, string closeMark, string htmlTag,
            List<Func<Tag, bool, string, bool>> textContentRules, 
            List<Func<Token, List<Token>, bool>> tagInteractionRules)
        {
            OpenMark = openMark;
            CloseMark = closeMark;
            HtmlTag = htmlTag;
            TextContentRules = Tag.DefaultTextContentRules.Concat(textContentRules).ToList();
            TagInteractionRules = Tag.DefaultTagInteractionRules.Concat(tagInteractionRules).ToList();
        }
    }
}
