using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Analyzer
    {
        private readonly HashSet<TextSelectionType> _FoundSelectionTypes;
        private readonly Stack<Tag> _currentTags;
        private readonly List<List<Tag>> _allTags;
        private List<Tag> _tempParagraphTags;
        private bool _isTagInWord { get; set; }
        private bool _isOnlyDigits { get; set; }

        public Analyzer()
        {
            _FoundSelectionTypes = new HashSet<TextSelectionType>();
            _currentTags = new Stack<Tag>();
            _allTags = new List<List<Tag>>();
            _tempParagraphTags = new List<Tag>();
        }

        public List<List<Tag>> GetTagsForAllParagraphs(string[] paragraphs)
        {
            for (var paragraphNumber = 0; paragraphNumber < paragraphs.Length; paragraphNumber++)
            {
                var paragraph = paragraphs[paragraphNumber];
                SearchTagsInParagraph(paragraph);
                _tempParagraphTags = _tempParagraphTags.OrderBy(tag => tag.Position).ToList();
                InsertMarkerContainer(paragraphNumber, paragraphs.Length - 1);
                _allTags.Add(_tempParagraphTags);
                ClearTempStorages();
            }

            return _allTags;
        }

        private void SearchTagsInParagraph(string paragraph)
        {
            var currentPosition = 0;
            while (currentPosition < paragraph.Length)
            {
                var currentMarkdownSymbol = GetMarkdownSymbol(paragraph, currentPosition);
                if (string.IsNullOrEmpty(currentMarkdownSymbol))
                    AnalyzeSimpleTextSymbol(paragraph, currentPosition);
                else
                {
                    currentPosition += currentMarkdownSymbol.Length - 1;
                    AnalyzeMarkdownSymbol(paragraph, currentPosition, currentMarkdownSymbol);
                }

                currentPosition++;
            }
        }

        private void AnalyzeSimpleTextSymbol(string paragraph, int currentPosition)
        {
            if (_isTagInWord && !paragraph.IsLetterInPosition(currentPosition))
            {
                _FoundSelectionTypes.Remove(_currentTags.Pop().SelectionType);
                _isTagInWord = false;
            }

            if (CheckSelectionForNotDigitInPosition(paragraph, currentPosition))
                _isOnlyDigits = false;
            if (_currentTags.Count != 0 && _currentTags.Peek().SelectionType == TextSelectionType.Slash)
                _FoundSelectionTypes.Remove(_currentTags.Pop().SelectionType);
        }

        private void AnalyzeMarkdownSymbol(string paragraph, int currentPosition, string currentMarkdownSymbol)
        {
            var selectionType = Comparison.GetTextSelectionType(currentMarkdownSymbol);
            if (IsMarkerOrHeaderSelection(paragraph, currentPosition, selectionType))
                AddFoundTagsInList(selectionType, currentPosition, paragraph.Length,
                    selectionType == TextSelectionType.Marker ? 2 : 1);
            else if (IsEscapingSlash())
            {
                _tempParagraphTags.Add(GetEscapingSlashTag());
                RemoveSpecifiedSelectionTypeAndAllBeforeIt(selectionType);
            }
            else if (paragraph.IsSlashInPosition(currentPosition))
                AddInformationAboutPossibleTag(typeof(SingleTag), @"\", currentPosition);
            else if (IsPossibleOpeningTag(currentMarkdownSymbol) &&
                     !paragraph.IsWhiteSpaceInPosition(currentPosition + 1))
            {
                _isOnlyDigits = true;
                AddInformationAboutPossibleTag(typeof(StartTag), currentMarkdownSymbol, currentPosition);
                if (paragraph.IsLetterInPosition(currentPosition - currentMarkdownSymbol.Length))
                    _isTagInWord = true;
            }
            else if (IsPossibleClosingTag(currentMarkdownSymbol))
            {
                if (IsNotValidClosingTag(paragraph, currentPosition, currentMarkdownSymbol))
                    RemoveSpecifiedSelectionTypeAndAllBeforeIt(selectionType);
                else
                {
                    var tag = _currentTags.Pop();
                    _FoundSelectionTypes.Remove(tag.SelectionType);
                    AddFoundTagsInList(tag.SelectionType, tag.Position - tag.Length + 1,
                        currentPosition - currentMarkdownSymbol.Length + 1, currentMarkdownSymbol.Length);
                    if (_isTagInWord)
                        _isTagInWord = false;
                }
            }
        }

        private void InsertMarkerContainer(int paragraphNumber, int lastParagraphNumber)
        {
            if (IsMarkedListBeginning())
                _tempParagraphTags.Insert(0, new StartTag(TextSelectionType.MarkContainer, 0, 0));

            if (IsLastParagraphContainsMarker(paragraphNumber, lastParagraphNumber))
            {
                _tempParagraphTags.Insert(_tempParagraphTags.Count,
                    new EndTag(TextSelectionType.MarkContainer,
                        _tempParagraphTags.Last().Position + _tempParagraphTags.Last().Length, 0));
            }
            else if (IsNotCurrentParagraphContainsMarker() && IsPreviousParagraphContainsMarker())
            {
                var previousParagraphTags = _allTags.Last();
                previousParagraphTags.Insert(previousParagraphTags.Count,
                    new EndTag(TextSelectionType.MarkContainer,
                        previousParagraphTags.Last().Position + previousParagraphTags.Last().Length, 0));
            }
        }

        private bool IsMarkedListBeginning()
        {
            return _tempParagraphTags.Count != 0 && _tempParagraphTags[0].SelectionType == TextSelectionType.Marker &&
                   (_allTags.Count == 0 || _allTags.Last().Count == 0
                                        || _allTags.Last()[0].SelectionType != TextSelectionType.Marker
                                        && _allTags.Last()[0].SelectionType != TextSelectionType.MarkContainer);
        }

        private bool IsPreviousParagraphContainsMarker()
        {
            return _allTags.Count != 0 && _allTags.Last().Last().SelectionType == TextSelectionType.Marker;
        }

        private bool IsNotCurrentParagraphContainsMarker()
        {
            return (_tempParagraphTags.Count == 0 ||
                    _tempParagraphTags.Last().SelectionType != TextSelectionType.Marker);
        }

        private bool IsLastParagraphContainsMarker(int paragraphNumber, int lastParagraphNumber)
        {
            return paragraphNumber == lastParagraphNumber && _tempParagraphTags.Count != 0 &&
                   _tempParagraphTags.Last().SelectionType == TextSelectionType.Marker;
        }

        private void ClearTempStorages()
        {
            _tempParagraphTags = new List<Tag>();
            _currentTags.Clear();
            _FoundSelectionTypes.Clear();
        }

        private bool IsNotValidClosingTag(string paragraph, int currentPosition, string currentMarkdownSymbol)
        {
            return (paragraph.IsWhiteSpaceInPosition(currentPosition - currentMarkdownSymbol.Length)
                    || IsDifferentSelectionTypesAtStartAndEndTags(currentMarkdownSymbol)
                    || IsEmptyInsideSelection(currentPosition, currentMarkdownSymbol) ||
                    _isOnlyDigits && IsBoldOrItalic());
        }

        private bool IsMarkerOrHeaderSelection(string paragraph, int currentPosition, TextSelectionType selectionType)
        {
            return currentPosition == 0 && (IsMarkerSelection(paragraph, currentPosition, selectionType) ||
                                            IsHeaderSelection(paragraph, currentPosition, selectionType));
        }

        private static bool IsMarkerSelection(string paragraph, int position, TextSelectionType selectionType)
        {
            return paragraph.Length > 2 && selectionType == TextSelectionType.Marker &&
                   paragraph.IsWhiteSpaceInPosition(1);
        }

        private static bool IsHeaderSelection(string paragraph, int position, TextSelectionType selectionType)
        {
            return paragraph.Length > 1 && selectionType == TextSelectionType.Header;
        }

        private bool IsPossibleOpeningTag(string checkingTeg)
        {
            return !_FoundSelectionTypes.Contains(
                       Comparison.GetTextSelectionType(checkingTeg)) &&
                   (_currentTags.Count == 0 || _currentTags.Peek().SelectionType
                       .IsCompatibleWith(
                           Comparison.GetTextSelectionType(checkingTeg)));
        }

        private bool IsPossibleClosingTag(string checkingTeg)
        {
            return _FoundSelectionTypes.Contains(Comparison.GetTextSelectionType(checkingTeg));
        }

        private static bool IsPossibleTag(string checkingTeg) => Comparison.IsMarkdownSymbol(checkingTeg);

        private void AddFoundTagsInList(TextSelectionType type, int startPosition, int endPosition, int length)
        {
            _tempParagraphTags.Add(new StartTag(type, startPosition, length));
            _tempParagraphTags.Add(new EndTag(type, endPosition, length));
        }

        private void AddInformationAboutPossibleTag(Type tagType, string currentMarkdownSymbol, int position)
        {
            var selectionType = Comparison
                .GetTextSelectionType(currentMarkdownSymbol);
            _FoundSelectionTypes.Add(selectionType);
            if (tagType == typeof(StartTag))
                _currentTags.Push(new StartTag(selectionType, position, currentMarkdownSymbol.Length));
            else
                _currentTags.Push(new SingleTag(selectionType, position, currentMarkdownSymbol.Length));
        }

        private static string GetMarkdownSymbol(string paragraph, int position)
        {
            if (position < paragraph.Length - 1 &&
                IsPossibleTag(paragraph[position] + paragraph[position + 1].ToString()))
                return paragraph[position] + paragraph[position + 1].ToString();
            if (position < paragraph.Length && IsPossibleTag(paragraph[position].ToString()))
                return paragraph[position].ToString();
            return null;
        }

        private void RemoveSpecifiedSelectionTypeAndAllBeforeIt(TextSelectionType currentSelectionType)
        {
            if (_currentTags.Count == 0)
                return;
            while (_currentTags.Count != 0 && _currentTags.Peek().SelectionType != currentSelectionType)
                _FoundSelectionTypes.Remove(_currentTags.Pop().SelectionType);
            _FoundSelectionTypes.Remove(_currentTags.Pop().SelectionType);
        }

        private bool IsDifferentSelectionTypesAtStartAndEndTags(string currentMarkdownSymbol)
        {
            return _currentTags.Peek().SelectionType != Comparison.GetTextSelectionType(currentMarkdownSymbol);
        }

        private bool IsBoldOrItalic()
        {
            return _currentTags.Peek().SelectionType == TextSelectionType.Bold ||
                   _currentTags.Peek().SelectionType == TextSelectionType.Italic;
        }

        private bool CheckSelectionForNotDigitInPosition(string paragraph, int position)
        {
            return (_FoundSelectionTypes.Contains(TextSelectionType.Italic) ||
                    _FoundSelectionTypes.Contains(TextSelectionType.Bold)) && !paragraph.IsDigitInPosition(position);
        }

        private bool IsEmptyInsideSelection(int position, string currentMarkdownSymbol)
        {
            return position - currentMarkdownSymbol.Length == _currentTags.Peek().Position;
        }

        private bool IsEscapingSlash()
        {
            return _currentTags.Count != 0 && _currentTags.Peek().SelectionType == TextSelectionType.Slash;
        }

        private Tag GetEscapingSlashTag()
        {
            var tag = _currentTags.Peek();
            _FoundSelectionTypes.Remove(_currentTags.Pop().SelectionType);
            return tag;
        }
    }
}