using System;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string input)
        {
            var tokenaizer = new Tokenizer(
                new MdWrapSetting("#", "<h1>", MdWrapType.Paragraph),
                new MdWrapSetting("##", "<h2>", MdWrapType.Paragraph),
                new MdWrapSetting("_", "<em>", MdWrapType.Text),
                new MdWrapSetting("__", "<strong>", MdWrapType.Text));

            return tokenaizer.Tokenize(input).Select(token => token.Render())
                .Aggregate((s1, s2) => s1 + Environment.NewLine + s2);
        }
    }
}