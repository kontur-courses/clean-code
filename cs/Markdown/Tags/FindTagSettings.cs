using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class FindTagSettings
    {
        public bool SearchForHeading;
        public bool SearchForBold;
        public bool SearchForItalic;

        public FindTagSettings(bool heading, bool bold, bool italic)
        {
            SearchForHeading = heading;
            SearchForBold = bold;
            SearchForItalic = italic;
        }
    }
}
