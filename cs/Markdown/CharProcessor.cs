using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class CharProcessor
    {
        private int LastUnderscoreIndex;
        private int LastTwoUnderScoreIndex;
        private int LastApostropheIndex;
        private int CurrentIndex;
        private bool UndercsoreFound;
        private bool TwoundercsoreFound;
        //private bool ClosingUnderscoreFound;
        private bool ApostropheFound;
        private bool isDigitInWord;
        private bool potentiallyClosingUnderscore;
        private char LastChar;
        private readonly FixedSizeQueue<char> lastNchars;//здесь будет очередь фиксированной длины в которой будут содержаться последние N символов
        private readonly Dictionary<int, Tag> toInsertIntoParagraph;

        public CharProcessor()
        {
            this.LastUnderscoreIndex = -1;
            this.LastTwoUnderScoreIndex = -1;
            this.LastApostropheIndex = -1;
            this.CurrentIndex = 0;
            this.UndercsoreFound = false;
            this.TwoundercsoreFound = false;
            this.ApostropheFound = false;
            this.isDigitInWord = false;
            this.potentiallyClosingUnderscore = false;
            this.lastNchars = new FixedSizeQueue<char>(3);
            this.toInsertIntoParagraph = new Dictionary<int, Tag>();
        }
        public void ProcessChar(char letter)
        {
            if (letter == default)
                ProcessEndOfParagraph();
            if (char.IsDigit(letter))
                ProcessDigit();
            if (potentiallyClosingUnderscore)
                ProcessPotentiallyClosingUnderscore(letter);
            else
            {
                switch (letter)
                {
                    case '_':
                        ProcessUnderscore();
                        break;
                    case '/':
                        ProcessSlash();
                        break;
                    case ' ':
                        ProcessSpace();
                        break;
                    default:
                        ProcessLetter();
                        break;
                }
            }
            lastNchars.Enqueue(letter);
            LastChar = letter;
            CurrentIndex++;
        }

        private void ProcessUnderscore()
        {
            if (isDigitInWord)
                return;
            if (!UndercsoreFound && !TwoundercsoreFound)
            {
                UndercsoreFound = true;
                LastUnderscoreIndex = CurrentIndex;
                return;
            }
            if (UndercsoreFound && !TwoundercsoreFound)
            {
                if (lastNchars.Last() == ' ')
                    return;
                if (LastUnderscoreIndex==CurrentIndex-1)
                {
                    UndercsoreFound = false;
                    TwoundercsoreFound = true;
                    LastTwoUnderScoreIndex = CurrentIndex;
                }
                else
                {
                    if (lastNchars.Last() == '_')
                        TwoundercsoreFound = true;
                    else
                        potentiallyClosingUnderscore = true;
                }
            }
            if (!UndercsoreFound && TwoundercsoreFound)
            {

                if (LastTwoUnderScoreIndex==CurrentIndex-1)
                {
                    LastTwoUnderScoreIndex = CurrentIndex;
                }
                else
                {
                    potentiallyClosingUnderscore = true;
                }
            }
        }

        private void ProcessSpace()
        {
            if (isDigitInWord)
                isDigitInWord = false;
        }

        private void ProcessDigit()
        {
            isDigitInWord = true;
        }

        private void ProcessLetter()
        {

        }

        private void ProcessApostrophe()
        {
            throw new NotImplementedException();
        }

        private void ProcessSlash()
        {
            throw new NotImplementedException();
        }


        private void ProcessPotentiallyClosingUnderscore(char letter)
        {
            potentiallyClosingUnderscore = false;
            if (UndercsoreFound && letter!='_')
            {
                toInsertIntoParagraph.Add(LastUnderscoreIndex, Tag.Em);
                toInsertIntoParagraph.Add(CurrentIndex-1, Tag.Em_close);
                UndercsoreFound = false;
            }
            if (UndercsoreFound && letter=='_')
            {
            }
            if (TwoundercsoreFound && letter=='_')
            {
                toInsertIntoParagraph.Add(LastTwoUnderScoreIndex - 1, Tag.Strong);
                toInsertIntoParagraph.Add(CurrentIndex - 2, Tag.Strong_close);
                TwoundercsoreFound = false;
            }
            ProcessChar(letter);
        }

        private void ProcessEndOfParagraph()
        {
            if (potentiallyClosingUnderscore && UndercsoreFound)
            {
                toInsertIntoParagraph.Add(LastUnderscoreIndex, Tag.Em);
                toInsertIntoParagraph.Add(CurrentIndex-1, Tag.Em_close);
                UndercsoreFound = false;
            }
        }

        public Dictionary<int, Tag> GetInserts()
        {
            return this.toInsertIntoParagraph;
        }
    }
}
