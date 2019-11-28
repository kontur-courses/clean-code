using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Renderer
    {        
        public string Render(string text)
        {
            var zed = new Tokenizer();            
            var m = zed.GetTokens(text);            
            return new RendererToHTML().ToHTML(text, m);            
        }
    }
}
