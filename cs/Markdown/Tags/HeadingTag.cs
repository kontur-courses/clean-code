using System;

namespace Markdown.Tags
{
    public class HeadingTag : Tag
    {
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override string OpenMdTag => "# ";
        public override string CloseMdTag => "\n";
        public override bool AllowNesting => true;
        public override Func<string, int, bool> IsCorrectOpenTag => (mdText, position) => true;
        public override Func<string, int, bool> IsCorrectCloseTag => (mdText, position) =>
        true;
    }
}
