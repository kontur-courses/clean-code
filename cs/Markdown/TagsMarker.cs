using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class TagsMarker
    {
        public static List<TagWithToken> TokensToTagWithTokens(List<Token> tokens, string text, List<Tag> tags)
        {
            var openingTagList = new List<TagWithToken>();
            var tagWithTokens = new List<TagWithToken>();
            foreach (var token in tokens)
            {
                if (token.IsTag)
                {
                    var tag = tags.Find(t => t.MatchTagAndTokenCompletely(token.Index, token.Length, text));
                    var tagWithToken = new TagWithToken(tag, token);
                    if (tagWithToken.CanBeTag(text))
                    {
                        tagWithTokens.Add(tagWithToken);
                        HandlerTag(openingTagList, tagWithTokens, tagWithToken, text);
                    }
                    else
                        tagWithTokens.Add(new TagWithToken(null, token));
                }
                else
                {
                    tagWithTokens.Add(new TagWithToken(null, token));
                }
            }
            return tagWithTokens;
        }

        private static void HandlerTag(List<TagWithToken> openingTagList, List<TagWithToken> tagWithTokens, TagWithToken tagWithToken, string text)
        {
            if (tagWithToken.CanTagBeClosing(text))
            {
                var indexOpeningTag = openingTagList.FindLastIndex(tag => tagWithToken.Tag.MarkdownTag == tag.Tag.MarkdownTag && tag.CanTagBeOpening(text));
                if (indexOpeningTag >= 0)
                {
                    openingTagList[indexOpeningTag].IsOpen = true;
                    tagWithToken.IsClose = true;
                    openingTagList.RemoveRange(indexOpeningTag, openingTagList.Count - indexOpeningTag);

                    switch (tagWithToken.Tag.MarkdownTag)
                    {
                        case "_":
                            var i = tagWithTokens.Count - 1;
                            while (tagWithTokens[i] != tagWithTokens[indexOpeningTag])
                            {
                                if (tagWithTokens[i].IsTag && tagWithTokens[i].Tag.MarkdownTag == "__")
                                {
                                    tagWithTokens[i].IsClose = false;
                                    tagWithTokens[i].IsOpen = false;
                                }
                                i--;
                            }
                            break;
                    }
                    return;
                }
            }
            if (tagWithToken.CanTagBeOpening(text))
            {
                openingTagList.Add(tagWithToken);
            }
        }
    }
}
