using System.Collections.Generic;
using Markdown.DTOs;
using Markdown.Tags;

namespace Markdown.Validators
{
    public class MDValidator : Validator
    {
        public override bool IsEscaped(int start, string input)
        {
            return start - 1 >= 0 ? input[start - 1] == '\\' : false;
        }

        public override bool TryGetCorrectTagContainer(int start, string input, out TagTypeContainer tagContainer,
            ITag tag)
        {
            if (start != 0)
            {
                if (start + tag.Length < input.Length) // "__ "
                {
                    if (isOpen[tag.StringTag])
                    {
                        if (input[start - 1] != ' ')
                        {
                            tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], start); // b_ closer
                            return true;
                        }
                    }

                    if (input[start + tag.Length] != ' ')
                    {
                        tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], start); // opener
                        return true;
                    }
                }

                if (start + tag.Length == input.Length) //"__"
                {
                    if (isOpen[tag.StringTag])
                    {
                        if (input[start - 1] != ' ')
                        {
                            tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], start); // closer
                            return true;
                        }
                    }

                    tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], start); // opener
                    return true;
                }
            }

            if (input[start + tag.Length] != ' ')
            {
                tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], start); // opener
                return true;
            }

            tagContainer = null;
            return false;
        }

        public override bool TryGetTag(int start, string input, out ITag tag)
        {
            for (var j = 0; j < tags.Count; j++)
            {
                var currentTag = tags[j];
                var k = 0;
                if (currentTag.Length + start > input.Length)
                    continue;
                for (var i = 0; i < currentTag.Length; i++)
                {
                    if (input[start + k] == currentTag.StringTag[i])
                        k += 1;
                    else
                        break;
                }

                if (k == currentTag.Length)
                {
                    tag = currentTag;
                    return true;
                }
            }

            tag = null;
            return false;
        }

        public MDValidator(Dictionary<string, bool> openChecker, List<ITag> tagsList) : base(openChecker, tagsList)
        {
        }
    }
}