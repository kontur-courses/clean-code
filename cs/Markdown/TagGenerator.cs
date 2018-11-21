using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class TagGenerator
    {
        private static readonly Dictionary<TagNames, OpeningAndClosingTagPair<string, string>>
            OpeningAndClosingTagPairs;

        static TagGenerator()
        {
            OpeningAndClosingTagPairs = new Dictionary<TagNames, OpeningAndClosingTagPair<string, string>>
            {
                {TagNames.Em, new OpeningAndClosingTagPair<string, string>("<em>", "</em>")},
                {TagNames.Strong, new OpeningAndClosingTagPair<string, string>("<strong>", "</strong>")}
            };
        }

        public static bool TryCreateTag(string paragraph, int position, out string tag)
        {
            if (StringScanner.IsStrongTag(paragraph, position))
            {
                if (StringScanner.IsClosingTag(paragraph, position))
                {
                    var isSuccess = OpeningAndClosingTagPairs.TryGetValue(TagNames.Em, out var tagPair);
                    if (isSuccess)
                    {
                        tag = tagPair.Closing;
                        return true;
                    }
                    else
                    {
                        tag = String.Empty;
                        return false;
                    }
                }
                else if (StringScanner.IsOpeningTag(paragraph, position))
                {
                    var isSuccess = OpeningAndClosingTagPairs.TryGetValue(TagNames.Strong, out var tagPair);
                    if (isSuccess)
                    {
                        tag = tagPair.Opening;
                        return true;
                    }
                    else
                    {
                        tag = String.Empty;
                        return false;
                    }
                }
            }
            else if (StringScanner.IsEmTag(paragraph, position))
            {
                if (StringScanner.IsClosingTag(paragraph, position))
                {
                    var isSuccess = OpeningAndClosingTagPairs.TryGetValue(TagNames.Em, out var tagPair);
                    if (isSuccess)
                    {
                        tag = tagPair.Closing;
                        return true;
                    }
                    else
                    {
                        tag = String.Empty;
                        return false;
                    }
                }
                else if (StringScanner.IsOpeningTag(paragraph, position))
                {
                    var isSuccess = OpeningAndClosingTagPairs.TryGetValue(TagNames.Em, out var tagPair);
                    if (isSuccess)
                    {
                        tag = tagPair.Opening;
                        return true;
                    }
                    else
                    {
                        tag = String.Empty;
                        return false;
                    }
                }
            }
             
            tag = string.Empty;
            return false;
        }
    }

    enum TagNames
    {
        Em,
        Strong = 2
    }
}
