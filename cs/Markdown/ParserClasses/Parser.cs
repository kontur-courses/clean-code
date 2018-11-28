using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses
{
    public class Parser
    {
        public SentenceNode Parse(Deque<Token> tokens)
        {
            var sentence = ParseSentence(tokens);

            return sentence;
        }

        public SentenceNode ParseSentence(Deque<Token> tokens)
        {
            var sentence = new SentenceNode();

            while (tokens.Count > 0)
            {
                var currentToken = tokens.PeekFirst();
                TextNode newText;

                switch (currentToken.Type)
                {
                    case TokenType.Text:
                    case TokenType.Num:
                    case TokenType.Space:
                        newText = ParseText(tokens);
                        sentence.Add(newText);
                        break;
                    case TokenType.Underscore:
                        newText = ParseEmphasisText(tokens);
                        sentence.Add(newText);
                        break;
                    case TokenType.DoubleUnderscore:
                        newText = ParseBoldText(tokens);
                        sentence.Add(newText);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return sentence;
        }

        public TextNode ParseBoldText(Deque<Token> tokens)
        {
            var doubleUnderscore = tokens.PopFirst();
            if (tokens.Count == 0)
                return ParseText(new Deque<Token> {new Token(TokenType.Text, doubleUnderscore.Value)});

            if (tokens.PeekFirst().Type == TokenType.Space)
            {
                tokens.Insert(0, new Token(TokenType.Text, doubleUnderscore.Value));
                return ParseText(tokens);
            }

            var boldText = new TextNode(TextType.Bold);

            while (true)
            {
                if (tokens.Count == 0)
                {
                    boldText.Words.Insert(0, new WordNode(WordType.SimpleWord, doubleUnderscore.Value));
                    return new TextNode(TextType.Text, boldText.Words);
                }

                var currentToken = tokens.PeekFirst();

                switch (currentToken.Type)
                {
                    case TokenType.DoubleUnderscore:
                        tokens.PopFirst();
                        if (boldText.Words.Last().Type == WordType.Space)
                        {
                            boldText.Words.Insert(0, new WordNode(WordType.SimpleWord, doubleUnderscore.Value));
                            return new TextNode(TextType.Text, boldText.Words);
                        }

                        return boldText;
                    case TokenType.Underscore:
                        var innerEmphasisText = ParseEmphasisText(tokens);
                        if (innerEmphasisText.Type == TextType.Text)
                        {
                            boldText.AddRange(innerEmphasisText.Words);
                            continue;
                        }

                        var openEmTag = new WordNode(WordType.SimpleWord, "<em>");
                        var closeEmTag = new WordNode(WordType.SimpleWord, "</em>");

                        boldText.Add(openEmTag);
                        boldText.AddRange(innerEmphasisText.Words);
                        boldText.Add(closeEmTag);
                        break;
                    default:
                        var innerText = ParseText(tokens);
                        var innerWords = innerText.Words;

                        boldText.AddRange(innerWords);
                        break;
                }
            }
        }

        public TextNode ParseEmphasisText(Deque<Token> tokens)
        {
            var underscore = tokens.PopFirst();
            if (tokens.Count == 0)
                return ParseText(new Deque<Token> {new Token(TokenType.Text, underscore.Value)});

            if (tokens.PeekFirst().Type == TokenType.Space)
            {
                tokens.Insert(0, new Token(TokenType.Text, underscore.Value));
                return ParseText(tokens);
            }

            var emphasisText = new TextNode(TextType.Emphasis);

            while (true)
            {
                if (tokens.Count == 0)
                {
                    emphasisText.Words.Insert(0, new WordNode(WordType.SimpleWord, underscore.Value));
                    return new TextNode(TextType.Text, emphasisText.Words);
                }

                var currentToken = tokens.PeekFirst();

                switch (currentToken.Type)
                {
                    case TokenType.Underscore:
                        tokens.PopFirst();
                        if (emphasisText.Words.Last().Type == WordType.Space)
                        {
                            emphasisText.Words.Insert(0, new WordNode(WordType.SimpleWord, underscore.Value));
                            return new TextNode(TextType.Text, emphasisText.Words);
                        }

                        return emphasisText;
                    case TokenType.DoubleUnderscore:
                        tokens.PopFirst();
                        var doubleUnderscore = new WordNode(WordType.SimpleWord, underscore.Value + underscore.Value);
                        emphasisText.Add(doubleUnderscore);
                        break;
                    default:
                        var innerText = ParseText(tokens);
                        var innerWords = innerText.Words;

                        emphasisText.AddRange(innerWords);
                        break;
                }
            }
        }

        public TextNode ParseText(Deque<Token> tokens)
        {
            var plainText = new TextNode();

            while (true)
            {
                if (tokens.Count == 0)
                    break;

                var currentToken = tokens.PeekFirst();

                if (currentToken.Type == TokenType.EscapeChar)
                    throw new NotSupportedException();

                var type = currentToken.Type;
                if (type != TokenType.Space
                    && type != TokenType.Text
                    && type != TokenType.Num)
                {
                    break;
                }

                var newWord = currentToken.Type == TokenType.Space ? ParseSpacedWord(tokens) : ParseSimpleWord(tokens);
                plainText.Add(newWord);
            }

            return plainText;
        }

        public WordNode ParseSimpleWord(Deque<Token> tokens)
        {
            var simpleWord = tokens.PopFirst();

            return new WordNode(WordType.SimpleWord, simpleWord.Value);
        }

        public WordNode ParseSpacedWord(Deque<Token> tokens)
        {
            var space = tokens.PopFirst();

            if (tokens.Count == 0
                || (tokens.PeekFirst().Type != TokenType.Num && tokens.PeekFirst().Type != TokenType.Text))
                return new WordNode(WordType.Space, space.Value);

            var word = tokens.PopFirst();

            return new WordNode(WordType.SpacedWord, space.Value + word.Value);
        }
    }
}
