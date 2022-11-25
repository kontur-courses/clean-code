using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Exstensions
{
    public interface ITagCondition<TType> where TType : Enum
    {
        public void SetOpenIndex(TType type, int openIndex);
        public void SetTagOpenStatus(TType type, bool value);
        public int GetOpenIndex(TType type);
        public bool GetTagOpenStatus(TType type);
        public void OpenTag(TType type, int index);
        public void CloseTag(TType type);
        public string GetTag(TType type);
    }
}
