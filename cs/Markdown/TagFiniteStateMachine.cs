using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tag_Classes;

namespace Markdown
{
    public class TagFiniteStateMachine
    {
        private readonly List<TagEvent> tagEvents;
        private string _input;
        private State state;
        private Sign _sign;
        private StringBuilder text;

        public TagFiniteStateMachine()
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
            return tagEvents;
        }

        private void LaunchStateMachine()
        {
            for (var index = 0; index < _input.Length; index++)
            {
                var symbol = _input[index];
                _sign = GetSymbol(symbol);
                var tagEvent = CombineStateAndSymbol(symbol);
                if (tagEvent != null) tagEvents.Add(tagEvent);
            }
            AddTextToTagEventsIfNotEmpty();
        }

        private TagEvent CombineStateAndSymbol(char symbol)
        {
            switch (state)
            {
                case State.Other: return ProcessStateOther(symbol);
                case State.UnderlineBeginnig: return ProcessStateSingleUnderlineBeginnig(symbol);
                case State.UnderlineEnding: return ProcessStateTagEnding(symbol);
                case State.Whitespace: return ProcessStateWhitespace(symbol);
                default: throw new ArgumentException($"unrecognized state: {state}");
            }
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
                case Sign.Other:
                    text.Append(symbol);
                    state = State.Other;
                    break;
                default:
                    throw new ArgumentException($"unrecognized sign: {_sign}");
            }
            return tagEvent;
        }

        private TagEvent ProcessStateTagEnding(char symbol)
        {
            //пока в любом случае
            TagEvent tagEvent = null;
            switch (_sign)
            {
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

        private TagEvent ProcessStateSingleUnderlineBeginnig(char symbol)
        {
            TagEvent tagEvent = null;
            switch (_sign)
            {
                case Sign.Other:
                    text.Append(symbol);
                    break;
                case Sign.Whitespace:
                    text.Append(symbol);
                    state = State.Whitespace;
                    break;
                case Sign.Underline:
                    state = State.UnderlineEnding;
                    AddTextToTagEventsIfNotEmpty();
                    tagEvent = new TagEvent(Side.Right, Mark.Underliner, symbol.ToString());
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
                case Sign.Digit:
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
            }
            return tagEvent;
        }

        private Sign GetSymbol(char symbol)
        {
            if (char.IsDigit(symbol)) return Sign.Digit;
            switch (symbol)
            {
                case ' ': return Sign.Whitespace;
                case '_': return Sign.Underline;
                case '\\': return Sign.Escape;
                default: return Sign.Other;
            }
        }

        private void AddTextToTagEventsIfNotEmpty()
        {
            if (text.Length > 0)
            {
                tagEvents.Add(TagEvent.GetTextTagEvent(text.ToString()));
                text.Clear();
            }
        }
    }
}
