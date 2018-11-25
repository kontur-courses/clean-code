using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace Markdown
{
    public class Md
    {   
        private readonly ITokenMatcher[] orderedTags;
        
        public Md(ITokenMatcher[] orderedTagsMatchers)=>
            this.orderedTags = orderedTagsMatchers;
        
        public Md() =>
            this.orderedTags = new ITokenMatcher[]
            {
                new WrappingTokenMatcher("__", HtmlTextWriterTag.Strong),
                new WrappingTokenMatcher("_", HtmlTextWriterTag.U),
                new WrappingTokenMatcher("**", HtmlTextWriterTag.Strong),
                new WrappingTokenMatcher("*", HtmlTextWriterTag.U),
                new LineTokenMatcher("###",HtmlTextWriterTag.H3),
                new LineTokenMatcher("##",HtmlTextWriterTag.H2),
                new LineTokenMatcher("#",HtmlTextWriterTag.H1),
            };

        
        public string Render(string markdowned)
        {
            foreach (var matcher in orderedTags)
                matcher.TargetString = markdowned;
            
            var collector = new HtmlReplacingsCollector();
            collector.CollectTagPairs(markdowned, orderedTags);

            using (var applyer = new ReplacingsApplyer(markdowned))
            {
                applyer.Apply(collector.GetReplacings);
                return applyer.Result.ToString();
            }
        }

    }
}