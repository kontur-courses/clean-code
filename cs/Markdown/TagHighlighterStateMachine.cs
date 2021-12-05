using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tag_Classes;

namespace Markdown
{
    public class TagHighlighterStateMachine
    {
        private readonly List<TagEvent> tagEvents;
        private string _input;
        private State state;
        private Sign _sign;
        private StringBuilder text;

        public TagHighlighterStateMachine()
        {
            tagEvents = new List<TagEvent>();
            text = new StringBuilder();
            state = State.Other;
            _sign = Sign.Other;
        }

        public List<TagEvent> GetTagEvents(string input)
        {
            _input = input;
            LaunchStateMachine();
            FinishStateMachine();
            return tagEvents;
        }

        private void LaunchStateMachine()
        {
            foreach (var symbol in _input)
            {
                _sign = GetSign(symbol);
                var tagEvent = ProcessState(symbol);
                if (tagEvent != null) tagEvents.Add(tagEvent);
            }
        }

        private void FinishStateMachine()
        {
            AddEscapeToTagEvents();
            AddTextToTagEventsIfNotEmpty();
        }

        private TagEvent ProcessState(char symbol)
        {
            switch (state)
            {
                case State.Other: return ProcessStateOther(symbol);
                case State.UnderlineBeginnig: return ProcessStateUnderlineBeginnig(symbol);
                case State.UnderlineEnding: return ProcessStateUnderlineEnding(symbol);
                case State.Whitespace: return ProcessStateWhitespace(symbol);
                case State.Escape: return ProcessStateEscape(symbol);
                default: throw new ArgumentException($"unrecognized state: {state}");
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
                    state = State.UnderlineBeginnig;
                    break;
                case Sign.Escape:
                    text.Append(symbol);
                    state = State.Escape;
                    break;
                case Sign.Whitespace:
                    text.Append(symbol);
                    state = State.Whitespace;
                    break;
                case Sign.Other:
                    text.Append(symbol);
                    state = State.Other;
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
                    state = State.UnderlineBeginnig;
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Other:
                    text.Append(symbol);
                    state = State.Other;
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
                    state = State.Whitespace;
                    text.Append(symbol);
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Digit:
                    state = State.Digit;
                    text.Append(symbol);
                    break;
                case Sign.Other:
                    state = State.Other;
                    text.Append(symbol);
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
                    state = State.UnderlineEnding;
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Right, Mark.Underliner, symbol.ToString());
                    break;
                case Sign.Whitespace:
                    text.Append(symbol);
                    state = State.Whitespace;
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
                case Sign.Other:
                    text.Append(symbol);
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
                    text.Append(symbol);
                    break;
                case Sign.Whitespace:
                    state = State.Whitespace;
                    text.Append(symbol);
                    break;
                case Sign.Underline:
                    state = State.UnderlineBeginnig;
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Left, Mark.Underliner, symbol.ToString());
                    break;
                case Sign.Escape:
                    GoToEscapeState(symbol);
                    break;
            }
            return tagEvent;
        }



        private Sign GetSign(char symbol)
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
            var buffer = text.ToString();
            if (buffer != "\\") return;
            tagEvents.Add(new TagEvent(Side.None, Mark.Escape, text.ToString()));
            text.Clear();
        }

        private void GoToEscapeState(char symbol)
        {
            state = State.Escape;
            AddTextToTagEventsIfNotEmpty();
            text.Append(symbol);
        }

        private void AddTextToTagEventsIfNotEmpty()
        {
            var textAsString = text.ToString();
            if (textAsString.Length > 0)
            {
                tagEvents.Add(TagEvent.GetTextTagEvent(textAsString));
                text.Clear();
            }
        }
    }
}
