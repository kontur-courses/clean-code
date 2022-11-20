using System.Text;

namespace Markdown;

public class TextSegment
    {
        private readonly StringBuilder _content = new StringBuilder();
        private readonly List<(int index, TextSegment segment)> _innerSegmentsWithPosition = new();
        private readonly List<TextSegment> _parentSegments = new();
        private bool _isClosed = false;
        public Symbol StartSymbol { get; }
        public bool IsInsideWord { get; }

        public TextSegment(Symbol startSymbol, TextSegment? parentSegment, bool isInsideWord)
        {
            StartSymbol = startSymbol;
            IsInsideWord = isInsideWord;
            _content.Append(startSymbol);
            if (parentSegment != null)
            {
                _parentSegments.Add(parentSegment);
                foreach (var segment in parentSegment._parentSegments)
                    _parentSegments.Add(segment);
            }
        }

        public void Close()
        {
            if (StartSymbol == "#")
                _content.Replace("\n", "#", _content.Length - StartSymbol.Length, StartSymbol.Length);
            else
                AddSymbol(StartSymbol.Value);
            Console.WriteLine($"_content: {_content}");
            _isClosed = true;
        }
        
        public void AddSymbol(string symbol)
        {
            _content.Append(symbol);
        }
        
        public void AddSymbol(char symbol)
        {
            _content.Append(symbol);
        }

        public void AddInnerSegment(TextSegment segment)
        {
            var startIndexOfSegment = _content.Length;
            _innerSegmentsWithPosition.Add((startIndexOfSegment, segment));
        }

        public string Render()
        {
            RenderInnerSegments();
            if (StartSymbol == "_")
            {
                if (HasDigitsInContent())
                    return _content.ToString();
            }

            if (StartSymbol == "__" || StartSymbol == "_")
            {
                bool isSegmentEmpty = true;
                foreach (var chank in _content.GetChunks())
                {
                    if (chank.ToArray().Any(c => c != '_' && char.IsWhiteSpace(c) == false))
                    {
                        isSegmentEmpty = false;
                        break;
                    }
                }

                if (isSegmentEmpty)
                    return _content.ToString();
            }
            if (StartSymbol == "__" && _parentSegments.Any(segment => segment.StartSymbol == "_"))
                return _content.ToString();
            if (StartSymbol == "#")
            {
                _content.Append("#");
                _isClosed = true;
            }
            if (_isClosed)
                ReplaceBorderMarkdownSymbolsToTags();
            return _content.ToString();
        }

        private void RenderInnerSegments()
        {
            var sortedInnerSegments = _innerSegmentsWithPosition.OrderBy(segment => segment.index);
            var segmentShift = 0;
            
            foreach (var segmentWithPosition in sortedInnerSegments)
            {
                var startContentLength = _content.Length;
                var segmentIndex = segmentWithPosition.index + segmentShift;
                var segmentContent = segmentWithPosition.segment.Render();
                Console.WriteLine($"#1{_content}");
                _content.Insert(segmentIndex, segmentContent);
                Console.WriteLine($"#2{_content}");
                segmentShift += _content.Length - startContentLength;
            }
        }

        private string RenderWithoutReplacingMarkdownSymbols()
        {
            RenderInnerSegments();
            return _content.ToString();
        }

        private bool HasDigitsInContent()
        {
            return _content.ToString().Any(char.IsDigit);
        }

        private void ReplaceBorderMarkdownSymbolsToTags()
        {
            _content.Replace(StartSymbol.Value, StartSymbol.RelatedTag, 0, StartSymbol.Length).
                Replace(StartSymbol.Value, StartSymbol.RelatedCloseTag, _content.Length - StartSymbol.Length, StartSymbol.Length);
        }
    }
