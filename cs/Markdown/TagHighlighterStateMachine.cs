using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tag_Classes;

namespace Markdown
{
    public class TagHighlighterStateMachine
    {
        private readonly StringBuilder _text;
        private readonly List<TagEvent> _tagEvents;
        private string _input;
        private State _state;
        private Sign _sign;

        public TagHighlighterStateMachine()
        {
            _tagEvents = new List<TagEvent>();
            _text = new StringBuilder();
            _state = State.Other;
            _sign = Sign.Other;
        }

        public List<TagEvent> GetTagEvents(string input)
        {
            _input = input;
            LaunchStateMachine();
            FinishStateMachine();
            return _tagEvents;
        }

        private void LaunchStateMachine()
        {
            foreach (var symbol in _input)
            {
                _sign = GetSign(symbol);
                var tagEvent = ProcessState(symbol);
                if (tagEvent != null) _tagEvents.Add(tagEvent);
            }
        }

        private void FinishStateMachine()
        {
            AddEscapeToTagEvents();
            AddTextToTagEventsIfNotEmpty();
        }

        private TagEvent ProcessState(char symbol)
        {
            switch (_state)
            {
                case State.Other: return ProcessStateOther(symbol);
                case State.UnderlineBeginnig: return ProcessStateUnderlineBeginnig(symbol);
                case State.UnderlineEnding: return ProcessStateUnderlineEnding(symbol);
                case State.Whitespace: return ProcessStateWhitespace(symbol);
                case State.Escape: return ProcessStateEscape(symbol);
                default: throw new ArgumentException($"unrecognized _state: {_state}");
            }
        }

        private TagEvent ProcessStateEscape(char symbol)
        {
            TagEvent tagEvent = null;
            AddEscapeToTagEvents();
            switch (_sign)
            {
                case Sign.Underline:
                    tagEvent = new TagEvent(Side.Left, Mark.Underliner, symbol.ToString());
                    _state = State.UnderlineBeginnig;
                    break;
                case Sign.Escape:
                    _text.Append(symbol);
                    _state = State.Escape;
                    break;
                case Sign.Whitespace:
                    _text.Append(symbol);
                    _state = State.Whitespace;
                    break;
                case Sign.Other:
                    _text.Append(symbol);
                    _state = State.Other;
                    break;
            }
            return tagEvent;
        }

        private TagEvent ProcessStateWhitespace(char symbol)
        {
            TagEvent tagEvent = null;
            switch (_sign)
            {
                case Sign.Underline:
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Left, Mark.Underliner, symbol.ToString());
                    _state = State.UnderlineBeginnig;
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Other:
                    _text.Append(symbol);
                    _state = State.Other;
                    break;
                default:
                    throw new ArgumentException($"unrecognized sign: {_sign}");
            }
            return tagEvent;
        }

        private TagEvent ProcessStateUnderlineEnding(char symbol)
        {
            switch (_sign)
            {
                case Sign.Whitespace:
                    _state = State.Whitespace;
                    _text.Append(symbol);
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Digit:
                    _state = State.Digit;
                    _text.Append(symbol);
                    break;
                case Sign.Other:
                    _state = State.Other;
                    _text.Append(symbol);
                    break;
            }

            return null;
        }

        private TagEvent ProcessStateUnderlineBeginnig(char symbol)
        {
            TagEvent tagEvent = null;
            switch (_sign)
            {
                case Sign.Underline:
                    _state = State.UnderlineEnding;
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Right, Mark.Underliner, symbol.ToString());
                    break;
                case Sign.Whitespace:
                    _text.Append(symbol);
                    _state = State.Whitespace;
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Other:
                    _text.Append(symbol);
                    break;
                default:
                    throw new ArgumentException($"unrecognized sigh: {_sign}");
            }

            return tagEvent;
        }

        private TagEvent ProcessStateOther(char symbol)
        {
            TagEvent tagEvent = null;
            switch (_sign)
            {
                case Sign.Other:
                    _text.Append(symbol);
                    break;
                case Sign.Whitespace:
                    _state = State.Whitespace;
                    _text.Append(symbol);
                    break;
                case Sign.Underline:
                    _state = State.UnderlineBeginnig;
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Left, Mark.Underliner, symbol.ToString());
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
            }
            return tagEvent;
        }

        private static Sign GetSign(char symbol)
        {
            switch (symbol)
            {
                case ' ': return Sign.Whitespace;
                case '_': return Sign.Underline;
                case '\\': return Sign.Escape;
                default: return Sign.Other;
            }
        }

        private void AddEscapeToTagEvents()
        {
            var buffer = _text.ToString();
            if (buffer != "\\") return;
            _tagEvents.Add(new TagEvent(Side.None, Mark.Escape, _text.ToString()));
            _text.Clear();
        }

        private void GoToEscapeState(char symbol)
        {
            _state = State.Escape;
            AddTextToTagEventsIfNotEmpty();
            _text.Append(symbol);
        }

        private void AddTextToTagEventsIfNotEmpty()
        {
            var textAsString = _text.ToString();
            if (textAsString.Length <= 0) return;
            _tagEvents.Add(TagEvent.GetTextTagEvent(textAsString));
            _text.Clear();
        }
    }
}
