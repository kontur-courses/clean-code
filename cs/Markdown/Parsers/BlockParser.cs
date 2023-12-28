using Markdown.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsers
{
    public class BlockParser
    {
        private ITokenizer tokenizer;
        private Dictionary<TagType, int> toSkip;

        public BlockParser(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;

            toSkip = new Dictionary<TagType, int>()
            {
                { TagType.BulletList, Tokenizer.TypeToSymbols[TokenType.BulletList].Length },
            };
        }

        public MdDoc Parse(string text)
        {
            text = text.Replace("\n", @"\n");
            var lines = text.Split(@"\n");
            var mdDoc = new MdDoc();
            var lineParse = new LineParser();
            var prevIsBulletList = false;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var data = tokenizer.Tokenize(line);

                var currentIsBulletList = IsBulletList(data.tokens);
                if (currentIsBulletList)
                    data = CorrectData(data, TagType.BulletList);

                if (currentIsBulletList && !prevIsBulletList)
                    mdDoc.Tags.Add(new NestedTag(TagType.BulletList));

                var tag = lineParse.Parse(data.tokens, data.text, i == lines.Length - 1);

                if (currentIsBulletList)
                {
                    var row = new NestedTag(TagType.BulletListRow);
                    row.Tags.Add(tag);
                    mdDoc.Tags[^1].Tags.Add(row);
                }
                else
                    mdDoc.Tags.Add(tag);

                prevIsBulletList = currentIsBulletList;
            }

            return mdDoc;
        }

        (Token[] tokens, string text) CorrectData((Token[] tokens, string text) data, TagType type)
        {
            foreach (var token in data.tokens)
                token.Index -= toSkip[type];

            return (data.tokens[1..], data.text[toSkip[type]..]);
        }

        private bool IsBulletList(Token[] tokens)
        {
            return tokens.Length != 0 && tokens[0].Type == TokenType.BulletList; 
        }
    }
}
