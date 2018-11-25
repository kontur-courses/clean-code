using System.Collections.Generic;

namespace Markdown
{
    class StrongRegister : EmphasisRegister
    {
        public StrongRegister()
        {
            suffixLength = 2;
            suffixes = new []{ "**", "__" }; 
            priority = 1;
            tags = new [] { "<strong>", "</strong>" };
        }
    }
}
