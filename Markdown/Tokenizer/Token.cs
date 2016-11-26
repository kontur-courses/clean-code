using System.Collections.Generic;
using Markdown.Shell;

namespace Markdown.Tokenizer
{
    public class Token
    {
        public string Text { get; set; }
        public IShell Shell { get; }
        public List<Attribute> Attributes;
        public Token(string text, List<Attribute> attributes, IShell shell = null)
        {  
            Text = text;
            Shell = shell;
            Attributes = attributes;
        }

        public bool HasShell()
        {
            return Shell != null;
        }

    }
}
