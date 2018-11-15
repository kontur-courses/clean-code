using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;


namespace Markdown
{
    class Md
    {
        public Md()
        {
        }

        public string Render(string markDownInput)
        {
            var converter = new MdToHTMLConverter();
            var result = converter.Convert(markDownInput);
            return result;
        }
    }

    
}