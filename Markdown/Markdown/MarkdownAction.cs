using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public struct MarkdownAction
    {
        public MarkdownOperationType OType;
        public int Index;

        public MarkdownAction(MarkdownOperationType type, int index)
        {
            OType = type;
            Index = index;
        }
    }

    public enum MarkdownOperationType
    {
        OpenCursive,
        CloseCursive,
        OpenBold,
        CloseBold,
    }
}