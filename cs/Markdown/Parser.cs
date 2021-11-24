using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Markdown
{
    //---
    /*
    public class Parser
    {

        public IEnumerable<Token> Parse(string mdText)
        {
            var a = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением " +
                    "\r\n" +
                    "и остаются просто символами подчерка." +
                    "\r\n\r\n" +
                    "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения " +
                    "\r\n" +
                    "и остаются просто символами подчерка." +
                    "\r\n\r\n" +
                    "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."
            if (string.IsNullOrEmpty(mdText))
                throw new ArgumentException("Text should be not null and not empty");

            var paragraphs = SplitByParagraphs(mdText);
            foreach (var paragraph in paragraphs)
            {
                yield return GetParagraphOrHeader(paragraph);
            }
        }

        private string[] SplitByParagraphs(string mdText)
            => mdText.Split("\r\n");

        private Token GetParagraphOrHeader(string text)
        {
            Token token;
            if (text.Length > 0 && text[0] == '#')
                token = new Header(text.Substring(1, text.Length - 1));
            else
                token = new Paragraph(text);
            AddInnerTokensTo(token);
            return token;
        }

        private void AddInnerTokensTo(Token token)
        {
            var strongSplitted = SplitByTag(token.Value, "__");
            AddStrongTokensTo(token, strongSplitted);
            //Strong
        }

        private List<Token> SplitParagraph(Token token)
        {
            var tokens = token.Value.Split(" ")
                .Select(s => new Token(s))
                .ToList();
        }

        private List<Token> SplitByTokens(List<Token> tokens)
        {
            var tags = new List<string> {"_", "__"};

            foreach (var token in tokens)
            {
                if (token.Value.Contains("__") && !token.Value.ContainsDigit())
                {
                    GetStrong(token);
                }
                else if(token.Value.Contains("_") && !token.Value.ContainsDigit())
                {
                    
                }
            }
        }

        private Token GetStrong(Token token)
        {

        }

        private Token GetItalic(Token token)
        {

        }

        private Token GetEscaped(Token token)
        {
            var escaped
            var resValue = new StringBuilder();
            var isEscaped = false;
            var value = token.Value;
            for (var i = 0; i < value.Length; i++)
            {
                if (isEscaped)
                {

                }
                if (value[i] == '\\')
                {
                    isEscaped = true;
                    continue;
                }
            }
        }

        private string[] SplitByTag(string text, string tag)
        {
            var res = new string[0];
            if (tag.Length == 1)
                res = SplitByTagWithLength_1(text, tag);
            if(tag.Length == 2)
                res = SplitByTagWithLength_2(text, tag);
            return res;
        }

        private string[] SplitByTagWithLength_1(string splittingText, string tag)
        {
            var res = new List<string>();
            var text = new StringBuilder();
            foreach (var symbol in splittingText)
            {
                if (symbol.ToString() == tag)
                {
                    res.Add(text.ToString());
                    res.Add(tag);
                    text.Clear();
                }
                else
                    text.Append(symbol);
            }
            return res.ToArray();
        }

        private string[] SplitByTagWithLength_2(string splittingText, string tag)
        {
            var res = new List<string>();
            var text = new StringBuilder();
            var lastSymbols = "";
            foreach (var symbol in splittingText)
            {
                text.Append(symbol);
                var str = text.ToString();
                if (text.Length > 1)
                    lastSymbols = str.Substring(str.Length - 2, 2);
                if (lastSymbols == tag)
                {
                    res.Add(str.Substring(0, text.Length - 2));
                    res.Add(tag);
                    lastSymbols = "";
                    text.Clear();
                }
            }
            return res.ToArray();
        }

        private void AddItalicTokensTo(Token token, int position)
        {
            
        }

        private void AddStrongTokensTo(Token token, string[] str)
        {
            if(str.Length == 1)
                return;
            var inner = GetInnerTokens<StrongText>(str, "__");
            if (inner.Count == 0)
                return;
            token.InnerTokens = inner;
        }



        private List<Token> GetInnerTokens<T>(string[] str, string tag)
            where T : Token
        {
            List<Token> innerTokens = new List<Token>();
            var isTag = true;
            var previousTagsCount = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var previous = i == 0 ? null : str[i - 1];
                var current = str[i];
                var next = i == str.Length - 1 ? null : str[i + 1];
                if (current == "")
                {
                    isTag = true;
                    continue;
                }

                if (current == tag)
                    previousTagsCount++;
                else
                {
                    if (previous == tag && next == tag)
                    {
                        AddSequenceOfRepeatedTokens(innerTokens, previousTagsCount - 1, tag);
                        previousTagsCount = 0;
                        if (isTag)
                        {
                            innerTokens.Add(GetToken<T>(current));
                            i++;
                            isTag = false;
                        }
                        else
                        {
                            innerTokens.Add(new Token(current));
                            isTag = true;
                        }
                    }
                    else
                    {
                        innerTokens.Add(new Token(current));
                    }
                }
            }
            return innerTokens;




                //if (previous == "" && next == "" && current != "")
                //{
                //    innerTokens.Add(GetToken<T>(current));
                //}
                //else
                //{
                //    if (current == "")
                //        previousTagsCount++;
                //    else
                //    {
                //        innerTokens.Add(new Token(current));
                //    }
                //}



                //if (opened)
                //{
                //    if (text == "")
                //    {
                //        if(previousTagsCount == 0)
                //            innerTokens.Add(GetToken<T>(text));
                //        else
                //            previousTagsCount++;
                //    }
                //    else
                //    {
                //        AddSequenceOfRepeatedTokens(innerTokens, previousTagsCount + 1, tag);
                //        previousTagsCount = 0;
                //    }
                //}
                //else
                //{
                //    if (text == "")
                //    {
                //        opened = true;
                //    }
                //    else
                //    {
                //        innerTokens.Add(new Token(text));
                //    }
                //}

                //if (opened)
                //{
                //    if (text == "")
                //    {
                //        innerTokens.Add(new Token(tag + tag));
                //        opened = false;
                //        previous = text;
                //        continue;
                //    }
                //    innerTokens.Add(GetToken<T>(text));
                //}
                //else
                //    innerTokens.Add(new Token(text));

                //opened = text == "" && i != str.Length - 2;
                //previous = text;
            //}

            //return CollapseTokens(innerTokens);
        }

        private void AddSequenceOfRepeatedTokens
            (List<Token> tokens, int count, string repeatedValue)
        {
            if(count < 1)
                return;
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(repeatedValue);
            tokens.Add(new Token(sb.ToString()));
        }

        private List<Token> CollapseTokens(List<Token> tokens)
        {
            var result = new List<Token>();
            var collapsingTokens = new List<Token>();
            var previousToken = tokens[0];
            for (var i = 1; i < tokens.Count; i++)
            {
                var current = tokens[i];
                
                if (current.GetType() == previousToken.GetType())
                {
                    collapsingTokens.Add(previousToken);
                    previousToken = current;
                    continue;
                }
                if (collapsingTokens.Count != 0)
                {
                    collapsingTokens.Add(previousToken);
                    previousToken = CollapseSameTokens(collapsingTokens);
                    collapsingTokens = new List<Token>();
                }
                result.Add(previousToken);
                previousToken = current;
            }
            result.Add(previousToken);

            return result;
        }

        private T CollapseSameTokens<T>(List<T> tokens)
            where T: Token
        {
            var value = new StringBuilder();
            foreach (var token in tokens)
            {
                value.Append(token.Value);
            }
            return  GetToken<T>(value.ToString());
        }

        private T GetToken<T>(string value) where T : Token
            => (T) typeof(T)
                .GetConstructor(new[] {typeof(string) })
                .Invoke(new[] {value});



        /*
         private void AddItalicTokensTo(Token token, int position)
        {
            var tokens = new Stack<SequenceToken>();
            var match = "_";
            SequenceToken currentSequence = null;
            var text = token.Value;
            for (var i = position + 1; i < text.Length; i++)
            {
                var symbol = text[i].ToString();
                if (symbol == "\\")
                {
                    currentSequence = AddSymbol(currentSequence, symbol, i);

                }

                if (symbol == "_")
                {
                    currentSequence = currentSequence == null ?
                        new SequenceToken(symbol, i) : currentSequence.AddSymbols(symbol);
                }
            }
        }

        private SequenceToken AddSymbol(SequenceToken currentSequence, string symbol, int i)
            => currentSequence == null ?
                new SequenceToken(symbol, i) :
                currentSequence.AddSymbols(symbol);

    }
    */

    public class Parser
    {
        private readonly ParagraphParser paragraphParser = new ParagraphParser();

        public IEnumerable<Token> Parse(string mdText)
        {
            if (string.IsNullOrEmpty(mdText))
                throw new ArgumentException("Text should be not null and not empty");
            var paragraphs = SplitByParagraphs(mdText);
            foreach (var paragraph in paragraphs)
            {
                yield return GetParagraphOrHeader(paragraph);
            }
        }

        private string[] SplitByParagraphs(string mdText)
            => mdText.Split("\r\n");

        private Token GetParagraphOrHeader(string text)
        {
            Token token;
            if (text.Length > 0 && text[0] == '#')
                token = new Header(text.Substring(1, text.Length - 1));
            else
                token = new Paragraph(text);
            token.InnerTokens = paragraphParser.TokenizeParagraph(token.Value);
            return token;
        }
    }
}
