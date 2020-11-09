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
            public int Position;
            public string Text;
        }

        public List<TextElement> SplittedText { get; }

        public Tags()
        {
            SplittedText = new List<TextElement>();
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
