using System;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var rendered = new StringBuilder();
            var i = 0;
            while (i < text.Length) 
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

                    rendered.Append(new string('\\', count - count % 2));
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

                i++;
            }

            return rendered.ToString();
        }
    }
}