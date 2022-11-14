using System.Text;

namespace Markdown;

public class TextSegment
    {
        private StringBuilder _content;
        private List<TextSegment> _innerSegments;

        public int Length => _content.Length;

        public void AddSymbol(string symbol)
        {
            throw new NotImplementedException();
        }

        public void AddInnerSegment(TextSegment segment)
        {
            throw new NotImplementedException();
        }

        public string Render()
        {
            throw new NotImplementedException();
        }
    }
