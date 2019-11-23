using System;
using System.Collections.Generic;
using System.Text;
using Markdown.MdTags;

namespace Markdown.Builder
{
    internal class TagBuilder
    {
        private static readonly Dictionary<string, Func<string, int, Tag>> mdTags = new Dictionary<string, Func<string, int, Tag>>()
        {
            { "__", (text, index) =>new StrongTag(GetContent(text, index)) },
            { "_", (text, index) => new EmTag(GetContent(text, index)) },
            { "**", (text, index) => new StrongTag(GetContent(text, index)) },
            { "*", (text, index) => new ListTag(GetContent(text, index)) },
            { "~", (text, index) => new StrikeTag(GetContent(text, index)) },
            { "`", (text, index) => new CodeTag(GetContent(text, index)) },
            { "", (text, index) => new SimpleTag(GetContent(text, index)) },
            { "#", (text, index) => new HeaderTag(GetContent(text, index),"#" )},
            { "##", (text, index) => new HeaderTag(GetContent(text, index),"##") },
            { "###", (text, index) => new HeaderTag(GetContent(text, index),"###") },
            { "####", (text, index) => new HeaderTag(GetContent(text, index),"####") },
            { "#####", (text, index) => new HeaderTag(GetContent(text, index),"#####") },
            { "######", (text, index) => new HeaderTag(GetContent(text, index),"######") },
            { "\n", (text, index) => new HeaderTag(GetContent(text, index),"\n")},
            { "***", (text, index) => new HorizontalTag(GetContent(text, index)) },
            { "___", (text, index) => new HorizontalTag(GetContent(text, index)) },
            { ">", (text, index) => new BlockquoteTag(GetContent(text, index)) }
        };

        public static Tag BuildTag(string tag, string text, int offset)
            => mdTags[tag](text, offset);

        private static bool OtherTagFound(string text, int i)
            => Tag.AllTags.Contains(text[i].ToString());

        private static (int lenght, string content) GetContent(string text, int index)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (OtherTagFound(text, i))
                    break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, content, text[i + 1]);
                    continue;
                }

                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
        }

        private static void SlashHandler(ref int i, ref int length, StringBuilder content, char symbolToAdd)
        {
            content.Append(symbolToAdd);
            i++;
            length++;
        }
    }
}
