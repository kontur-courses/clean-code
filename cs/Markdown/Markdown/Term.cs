using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Term
    {
        private int startIndex;
        private int endIndex;
        public Term(int startIndex, int endIndex, string serviseSimb, bool isOpen = true)
        {
            ServiceSymbol = serviseSimb;
            IsOpen = isOpen;
            StartIndex = startIndex;
            EndIndex = endIndex;
            InnerTerms = new Stack<Term>();
        }
        public string ServiceSymbol { get; private set; }
        public bool IsOpen { get; private set; }

        public Stack<Term> InnerTerms;
        public int StartIndex 
        { 
            get
            {
                return startIndex;
            }
            set
            {
                if(value<0)
                    throw new ArgumentException();
                startIndex = value;
            }

        }

        public int EndIndex
        {
            get
            {
                return endIndex;
            }
            set
            {
                if (value < 0 )
                    throw new ArgumentException();
                endIndex = value;
            }

        }

        public void Close()
        {
            IsOpen = false;
        }

        public void ChangeServiseSymbol(string newSymbol)
        {
            ServiceSymbol = newSymbol;
        }

        public string GetInnerText(string input)
        {
            if (IsOpen)
                return input.Substring(StartIndex, EndIndex - StartIndex+1);
            return input.Substring(StartIndex + ServiceSymbol.Length, EndIndex - StartIndex-ServiceSymbol.Length+1);
        }
    }
}
