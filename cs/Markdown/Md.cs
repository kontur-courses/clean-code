using System;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var rendered = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var screened = false;
                if (text[i] == '\\')
                {
                    var count = 0;
                    while (i < text.Length && text[i] == '\\')
                    {
                        count++;
                        i++;
                    }

                    screened = count % 2 != 0;
                }
                if (TagBuilder.ExpectedToBeMark(text[i]))
                {
                    var tag = TagBuilder.BuildTag(text, i, screened);
                    rendered.Append(tag);
                    i = tag.ClosePosition;
                }
                else
                    rendered.Append(text[i]);
            }

            return rendered.ToString();
        }
    }
}