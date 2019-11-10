using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Md
    {
        private int LastUnderScoreIndex;
        private int LastTwoUnderScoreIndex;
        private int LastApostropheIndex;
        private int CurrentIndex;
        private bool UndercsoreFound;
        private bool TwoundercsoreFound;
        private bool ApostropheFound;
        private bool isDigitInWord;
        private readonly Dictionary<int, Tag> toInsertIntoParagraph;
        private readonly Queue<char> lastNchars;//здесь будет очередь фиксированной длины в которой будут содержаться последние N символов
        public String Render(String paragraph)
        {
            throw new NotImplementedException();
        }

        private void ProcessChar(char letter)
        {
            throw new NotImplementedException();
        }

        private void ProcessUnderscore()
        {
            throw new NotImplementedException();
        }

        private void ProcessSpace()
        {
            throw new NotImplementedException();
        }

        private void ProcessDigit()
        {
            throw new NotImplementedException();
        }

        private void ProcessLetter()
        {
            throw new NotImplementedException();
        }

        private void ProcessApostrophe()
        {
            throw new NotImplementedException();
        }

        private void ProcessSlash()
        {
            throw new NotImplementedException();
        }


    }

    enum Tag
    {
        Em,
        Em_close,
        Strong,
        Strong_close

    }

}
