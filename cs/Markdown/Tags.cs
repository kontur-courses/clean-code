using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tags
    {
        public struct TextElement
        {
            public int position;
            public string text;
        }

        public List<TextElement> splittedText { get; }

        public Tags()
        {
            splittedText = new List<TextElement>();
        }

        public void AddSharpTag(string text)
        {
            //преобразовавывает переданный текст в правильный HTML код и добавляет его в лист
        }

        public void AddStrongTag(string text)
        {
            //преобразовавывает переданный текст в правильный HTML код и добавляет его в лист
        }

        public void AddEMTag(string text)
        {
            //преобразовавывает переданный текст в правильный HTML код и добавляет его в лист
        }
    }


}
