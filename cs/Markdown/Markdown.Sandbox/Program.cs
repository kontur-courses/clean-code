using Markdown.Core;
using Markdown.Core.Helpers;

namespace Markdown.Sandbox
{
    public class Program
    {
        public static void Main()
        {
            var markdown = new Md(new MdTokenizer(), new MdParser(), new MdRender());
            markdown.Render("#123");
        }
    }
}