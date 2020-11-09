using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TagsFounder
    {
        public Tags Tags { get; }

        public TagsFounder(string text)
        {
            Tags = new Tags();
            FindTags(text);
        }

        private void FindTags(string text)
        {

        }
    }
}
