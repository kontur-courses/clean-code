using Markdown.Entities;
using System.Text;
using static Markdown.Inlines.InlineSyntax;

namespace Markdown.Inlines;

public class InlineParser
{
    private const int StrongLength = 2;
    private const int EmLength = 1;

    private bool _isStrongOpen;
    private bool _isEmOpen;

    private int _strongStartIndex;
    private int _emStartIndex;

    private bool _strongOpenedInsideWord;
    private bool _emOpenedInsideWord;

    private bool _strongSawWhitespace;
    private bool _emSawWhitespace;

    public IReadOnlyList<Node> Parse(string text)
    {
        ResetInlineMarkers();

        var input = InlineEscapesNormalizer.Normalize(text);

        var nodes = new List<Node>();
        var textBuilder = new StringBuilder(input.Length);

        for (int index = 0; index < input.Length;)
        {
            var current = input[index];

            if (current == Underscore)
            {
                index = HandleUnderscore(input, index, textBuilder, nodes);
                continue;
            }

            if (char.IsWhiteSpace(current))
                MarkWhitespace();

            textBuilder.Append(current);
            index++;
        }

        FinalizeUnclosedMarkers(textBuilder);
        textBuilder.CommitText(nodes);

        InlineEscapesNormalizer.RestorePlaceholders(nodes);
        nodes.MergeTextNodes();

        return nodes;
    }

    private int HandleUnderscore(string input, int index, StringBuilder textBuilder, List<Node> nodes)
    {
        var slice = input.AsSpan(index);

        if (slice.StartsWith(StrongMarker))
            return HandleStrong(input, index, textBuilder, nodes);

        return HandleEm(input, index, textBuilder, nodes);
    }

    private int HandleStrong(string input, int index, StringBuilder textBuilder, List<Node> nodes)
    {
        if (_isEmOpen && _isStrongOpen && _strongStartIndex < _emStartIndex)
        {
            textBuilder.InsertFromEnd(
            [
                (_strongStartIndex, StrongMarker),
                (_emStartIndex, EmMarker)
            ]);

            ResetInlineMarkers();

            textBuilder.Append(StrongMarker);
            return index + StrongLength;
        }

        if (_isEmOpen)
        {
            textBuilder.Append(StrongMarker);
            return index + StrongLength;
        }

        if (ShouldOpenStrong(input, index))
        {
            OpenStrong(input, index, textBuilder);
            return index + StrongLength;
        }

        if (ShouldCloseStrong(input, index, textBuilder))
        {
            CloseStrong(textBuilder, nodes);
            return index + StrongLength;
        }

        textBuilder.Append(StrongMarker);
        return index + StrongLength;
    }

    private int HandleEm(string input, int index, StringBuilder textBuilder, List<Node> nodes)
    {
        if (_isEmOpen && _emOpenedInsideWord && _emSawWhitespace)
        {
            textBuilder.Insert(_emStartIndex, EmMarker);
            _isEmOpen = false;
            _emSawWhitespace = false;

            textBuilder.Append(Underscore);
            return index + EmLength;
        }

        if (ShouldOpenEm(input, index))
        {
            OpenEm(input, index, textBuilder);
            return index + EmLength;
        }

        if (ShouldCloseEm(input, index, textBuilder))
        {
            CloseEm(textBuilder, nodes);
            return index + EmLength;
        }

        textBuilder.Append(Underscore);
        return index + EmLength;
    }

    private bool ShouldOpenStrong(string input, int index)
    {
        return !_isStrongOpen && CanOpenOrCloseMarker(text: input, position: index, length: 2, open: true);
    }

    private bool ShouldCloseStrong(string input, int index, StringBuilder textBuilder)
    {
        if (_isStrongOpen && _strongOpenedInsideWord && _strongSawWhitespace)
            return false;

        var nextChar = GetNextChar(text: input, position: index, length: 2);

        return _isStrongOpen &&
              CanOpenOrCloseMarker(text: input, position: index, length: 2, open: false) &&
              !IsCrossingWords(_strongOpenedInsideWord, char.IsLetterOrDigit(nextChar), _strongSawWhitespace) &&
              textBuilder.Length > _strongStartIndex;
    }

    private void OpenStrong(string input, int index, StringBuilder textBuilder)
    {
        _isStrongOpen = true;
        _strongStartIndex = textBuilder.Length;
        _strongOpenedInsideWord = char.IsLetterOrDigit(GetPrevChar(input, index));
        _strongSawWhitespace = false;
    }

    private void CloseStrong(StringBuilder textBuilder, List<Node> nodes)
    {
        var content = textBuilder.ToString(_strongStartIndex, textBuilder.Length - _strongStartIndex);
        textBuilder.Length = _strongStartIndex;

        textBuilder.CommitText(nodes);
        nodes.Add(new Node(content, NodeType.Strong));

        _isStrongOpen = false;
        _strongSawWhitespace = false;
    }

    private bool ShouldOpenEm(string input, int index)
    {
        return !_isEmOpen && CanOpenOrCloseMarker(input, index, 1, open: true);
    }

    private bool ShouldCloseEm(string input, int index, StringBuilder buffer)
    {
        var nextChar = GetNextChar(text: input, position: index, length: 1);

        return _isEmOpen &&
               CanOpenOrCloseMarker(input, index, 1, open: false) &&
               !IsCrossingWords(_emOpenedInsideWord, char.IsLetterOrDigit(nextChar), _emSawWhitespace) &&
               buffer.Length > _emStartIndex;
    }

    private void OpenEm(string input, int index, StringBuilder textBuilder)
    {
        _isEmOpen = true;
        _emStartIndex = textBuilder.Length;
        _emOpenedInsideWord = char.IsLetterOrDigit(GetPrevChar(input, index));
        _emSawWhitespace = false;
    }

    private void CloseEm(StringBuilder buffer, List<Node> nodes)
    {
        var emContent = buffer.ToString(_emStartIndex, buffer.Length - _emStartIndex);

        if (_isStrongOpen && _strongStartIndex < _emStartIndex)
        {
            var strongBeforeEm = buffer.ToString(_strongStartIndex, _emStartIndex - _strongStartIndex);
            buffer.Length = _strongStartIndex;
            buffer.CommitText(nodes);
            nodes.Add(new Node(strongBeforeEm, NodeType.Strong));
            _strongStartIndex = buffer.Length;
        }
        else
        {
            buffer.Length = _emStartIndex;
            buffer.CommitText(nodes);
        }

        nodes.Add(new Node(emContent, NodeType.Em));

        _isEmOpen = false;
        _emSawWhitespace = false;
    }

    private void ResetInlineMarkers()
    {
        _isStrongOpen = false;
        _isEmOpen = false;
        _strongOpenedInsideWord = false;
        _emOpenedInsideWord = false;
        _strongSawWhitespace = false;
        _emSawWhitespace = false;
        _strongStartIndex = 0;
        _emStartIndex = 0;
    }

    private void MarkWhitespace()
    {
        if (_isStrongOpen) _strongSawWhitespace = true;
        if (_isEmOpen) _emSawWhitespace = true;
    }

    private void FinalizeUnclosedMarkers(StringBuilder textBuilder)
    {
        if (!_isStrongOpen && !_isEmOpen)
            return;

        var inserts = new List<(int index, string text)>();

        if (_isStrongOpen)
            inserts.Add((_strongStartIndex, StrongMarker));

        if (_isEmOpen)
            inserts.Add((_emStartIndex, EmMarker));

        textBuilder.InsertFromEnd(inserts);

        ResetInlineMarkers();
    }
}