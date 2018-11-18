using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Translator
    {
        public static Dictionary<string, (string, string)> TranslateDictionary = new Dictionary<string, (string, string)>
        {
            {"_", ("<em>", "</em>" )},
            {"__", ("<strong>", "</strong>") }
        };
    }
}
