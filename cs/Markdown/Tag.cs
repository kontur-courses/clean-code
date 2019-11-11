using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Tag
    {
        public int MaxLength { get; } // максимальная длинна тега
        public string Html { get; } // Соответствуюший ему тэг из html

        public Tag(int maxLength, string html)
        {
            MaxLength = maxLength;
            Html = html;
        }
    }
}
