using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class UlTagConverter : TagConverterBase
    {
        protected override HashSet<string> TagInside => new HashSet<string>();

        public override string TagHtml => Markdown.TagHtml.ul;

        public override string TagName => MarkdownElement.list;

        public override bool IsTag(string text, int pos) => true;

        public override StringBuilder Convert(StringBuilder tagsText, StringBuilder text, int start, int finish)
        {
            var result = new StringBuilder();
            result.Append(TagName);
            foreach(var t in Split(tagsText, ';'))
            {
                result.Append(new LiTagConverter().FormTags(Markdown.Md.Render(t)));
            }
            result.Append(TagName);
            return FormTags(result);
        }

        private IEnumerable<StringBuilder> Split(StringBuilder text, char simbol)
        {
            var curentText = new StringBuilder();
            for(var i = TagName.Length; i < text.Length - TagName.Length; i++)
            {
                if(text[i] == Markdown.Md.shieldSimbol && i < text.Length - 1 && text[i + 1] == simbol) 
                {
                    curentText.Append(simbol);
                    i++;
                    continue;
                }
                if(text[i] == simbol && curentText.Length > 0)
                {
                    yield return curentText;
                    curentText.Clear();
                    continue;
                }
                curentText.Append(text[i]);
            }
            if (curentText.Length > 0)
                yield return curentText;
        }
    }
}
