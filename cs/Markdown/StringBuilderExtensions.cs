using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringBuilderExtensions
    {
        public static bool EndsWith(this StringBuilder self, string end)
        {
            if (self.Length < end.Length) return false;

            var j = 0;
            for (int i = self.Length - end.Length; i < self.Length; i++)
            {
                if (self[i] != end[j])
                    return false;
                j++;
            }
            return true;
        }
    }
}
