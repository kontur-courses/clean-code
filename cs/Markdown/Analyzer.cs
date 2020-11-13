using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Analyzer
    {
        private readonly HashSet<TextSelectionType> _FoundSelectionTypes;
        private readonly Stack<Tag> _startTags;

        public Analyzer()
        {
            _FoundSelectionTypes = new HashSet<TextSelectionType>();
            _startTags = new Stack<Tag>();
        }

        public List<IOrderedEnumerable<Tag>> GetTagsForAllParagraphs(string[] paragraphs)
        {
            return paragraphs.Select(paragraph => GetTagsForParagraph(paragraph).OrderBy(tag => tag.Position))
                .ToList();
        }

        private IEnumerable<Tag> GetTagsForParagraph(string paragraph)
        {
            var isTagInWord = false;
            var isOnlyDigits = false;
            var i = 0;
            while (i < paragraph.Length)
            {
                if (IsHeaderSelection(paragraph, i))
                {
                    yield return new StartTag(TextSelectionType.Header, 0, 1);
                    yield return new EndTag(TextSelectionType.Header, paragraph.Length, 1);
                    i++;
                    continue;
                }
                var currentMarkdownSymbol = GetMarkdownSymbol(paragraph, i);
                if (string.IsNullOrEmpty(currentMarkdownSymbol))
                {
                    if (_startTags.Count != 0 && isTagInWord && !paragraph.IsLetterInPosition(i))
                        _FoundSelectionTypes.Remove(_startTags.Pop().SelectionType);
                    if (CheckSelectionForNotDigitInPosition(paragraph, i))
                        isOnlyDigits = false;
                    if (_startTags.Count != 0 && _startTags.Peek().SelectionType == TextSelectionType.Slash)
                        _FoundSelectionTypes.Remove(_startTags.Pop().SelectionType);
                    i++;
                    continue;
                }
                i += currentMarkdownSymbol.Length - 1;
                if (IsPossibleOpeningTag(currentMarkdownSymbol))
                {
                    if (paragraph.IsWhiteSpaceInPosition(i + 1))
                    {
                        i++;
                        continue;
                    }
                    isOnlyDigits = true;
                    AddedInformationAboutPossibleTag(typeof(StartTag), currentMarkdownSymbol, i);
                    if (paragraph.IsLetterInPosition(i - currentMarkdownSymbol.Length))
                        isTagInWord = true;
                }
                else if (IsPossibleClosingTag(currentMarkdownSymbol))
                {
                    
                    if (paragraph.IsWhiteSpaceInPosition(i - currentMarkdownSymbol.Length) ||
                        IsDifferentSelectionTypesAtStartAndEndTags(currentMarkdownSymbol)
                        || IsEmptyInsideSelection(i, currentMarkdownSymbol) || isOnlyDigits && IsBoldOrItalic())
                    {
                        RemoveSpecifiedSelectionTypeAndAllBeforeIt(
                            Comparison.GetTextSelectionType(currentMarkdownSymbol));
                        i++;
                        continue;
                    }
                    var tag = _startTags.Pop();
                    _FoundSelectionTypes.Remove(tag.SelectionType);
                    yield return new StartTag(tag.SelectionType, tag.Position - tag.Length + 1,
                        currentMarkdownSymbol.Length);
                    yield return new EndTag(tag.SelectionType, i - currentMarkdownSymbol.Length + 1,
                        currentMarkdownSymbol.Length);
                }
                i++;
            }
        }
        
        private bool IsPossibleOpeningTag(string checkingTeg)
        {
            return IsPossibleTag(checkingTeg) &&
                   !_FoundSelectionTypes.Contains(
                       Comparison.GetTextSelectionType(checkingTeg)) &&
                   (_startTags.Count == 0 || _startTags.Peek().SelectionType
                       .IsCompatibleWith(
                           Comparison.GetTextSelectionType(checkingTeg)));
        }
        
        private bool IsPossibleClosingTag(string checkingTeg)
        {
            return IsPossibleTag(checkingTeg) &&
                   _FoundSelectionTypes.Contains(Comparison.GetTextSelectionType(checkingTeg));
        }
        
        private static bool IsPossibleTag(string checkingTeg) => Comparison.IsMarkdownSymbol(checkingTeg);
        
        private void AddedInformationAboutPossibleTag(Type tagType, string currentMarkdownSymbol, int position)
        {
            var selectionType = Comparison
                .GetTextSelectionType(currentMarkdownSymbol);
            _FoundSelectionTypes.Add(selectionType);
            if (tagType == typeof(StartTag))
                _startTags.Push(new StartTag(selectionType, position, currentMarkdownSymbol.Length));
            else
                _startTags.Push (new SingleTag(selectionType, position, currentMarkdownSymbol.Length));
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
            while (_startTags.Count != 0 && _startTags.Peek().SelectionType != currentSelectionType)
                _FoundSelectionTypes.Remove(_startTags.Pop().SelectionType);
            _FoundSelectionTypes.Remove(_startTags.Pop().SelectionType);
        }
        
        private bool IsDifferentSelectionTypesAtStartAndEndTags(string currentMarkdownSymbol)
        {
            return _startTags.Peek().SelectionType != Comparison.GetTextSelectionType(currentMarkdownSymbol);
        }

        private bool IsBoldOrItalic()
        {
            return _startTags.Peek().SelectionType == TextSelectionType.Bold ||
                   _startTags.Peek().SelectionType == TextSelectionType.Italic;
        }
        
        private bool CheckSelectionForNotDigitInPosition(string paragraph, int position)
        {
            return (_FoundSelectionTypes.Contains(TextSelectionType.Italic) ||
                    _FoundSelectionTypes.Contains(TextSelectionType.Bold)) && !paragraph.IsDigitInPosition(position);
        }
        
        private bool IsEmptyInsideSelection(int position, string currentMarkdownSymbol)
        {
            return position - currentMarkdownSymbol.Length == _startTags.Peek().Position;
        }
        
        private static bool IsHeaderSelection(string paragraph, int position)
        {
            var possibleHeader = paragraph[position].ToString();
            return position == 0 && paragraph.Length > 1 && Comparison.IsMarkdownSymbol(possibleHeader) &&
                   Comparison.GetTextSelectionType(possibleHeader) == TextSelectionType.Header;
        }
    }
}