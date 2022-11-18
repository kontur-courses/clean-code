using System.Collections.Generic;
using System.ComponentModel;

namespace Markdown
{
    public static class Tags
    {
        public static readonly 
            Dictionary<string, string> TagsDictionary = new Dictionary<string, string>()
            {
                ["_"] = "em",
                ["__"] = "strong",
                ["#"] = "title"
            };
    }
}