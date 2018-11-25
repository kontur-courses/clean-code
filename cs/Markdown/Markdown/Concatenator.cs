using System.Collections.Generic;
using System.Text;


namespace Markdown
{
    class Concatenator
    {
        private List<StringPart> _tmpList;
        private List<StringPart> _tmpOutput;
        private bool _tmpListIsEmpty;

        public Concatenator()
        {
            _tmpList = new List<StringPart>();
            _tmpOutput = new List<StringPart>();
            _tmpListIsEmpty = true;
        }

        public string Concatenate(IEnumerable<StringPart> paragraphParts)
        {
            paragraphParts = ReplaceTag(paragraphParts, TagType.Em);
            paragraphParts = ReplaceTag(paragraphParts, TagType.Strong);

            return ConcatenatePartsToString(paragraphParts);
        }

        public static string ConcatenatePartsToString(IEnumerable<StringPart> paragraphParts)
        {
            var builder = new StringBuilder();
            foreach (var part in paragraphParts)
                builder.Append(part.Value);

            return builder.ToString();
        }

        private List<StringPart> ReplaceTag(IEnumerable<StringPart> paragraphParts, TagType typeToReplace)
        {
            ClearTmpList();
            ClearTmpOutput();
            foreach (var part in paragraphParts)
                if (_tmpListIsEmpty)
                    HandleEmptyListCase(part, typeToReplace);
                else
                    HandleFilledListCase(part, typeToReplace);

            UploadResidualToTmpOutput(typeToReplace);

            var result = new List<StringPart>();
            result.AddRange(_tmpOutput);

            return result;
        }

        private void HandleEmptyListCase(StringPart part, TagType typeToReplace)
        {
            if (part.TagType != typeToReplace)
                _tmpOutput.Add(part);
            else
            {
                if (part.ActionType != ActionType.Close)
                {
                    _tmpListIsEmpty = false;
                    _tmpList.Add(part);
                }
                else
                    _tmpOutput.Add(new StringPart(part.Value));
            }
        }

        private void HandleFilledListCase(StringPart part, TagType typeToReplace)
        {
            if (part.TagType != typeToReplace)
                _tmpList.Add(part);
            else 
            {
                if (part.ActionType != ActionType.Open)
                {
                    UploadToTmpOutput(typeToReplace);
                    ClearTmpList();
                    _tmpListIsEmpty = true;
                }
                else
                    _tmpList.Add(new StringPart(part.Value));    
            }
        }

        private void UploadResidualToTmpOutput(TagType typeToReplace)
        {
            foreach (var part in _tmpList)
            {
                var item = part.TagType != typeToReplace ? part : new StringPart(part.Value);
                _tmpOutput.Add(item);
            }
            ClearTmpList();
        }

        private void UploadToTmpOutput(TagType typeToReplace)
        {

            var value = new StringBuilder();
            value.Append("<" + typeToReplace.ToString().ToLower() + ">");

            for (var i = 1; i < _tmpList.Count; i++)
                value.Append(_tmpList[i].Value);

            value.Append("</" + typeToReplace.ToString().ToLower() + ">");

            _tmpOutput.Add(new StringPart(value.ToString()));
        }

        private void ClearTmpList()
        {
            _tmpList = new List<StringPart>();
            _tmpListIsEmpty = true;
        }

        private void ClearTmpOutput()
        {
            _tmpOutput = new List<StringPart>();
        }
    }
}
