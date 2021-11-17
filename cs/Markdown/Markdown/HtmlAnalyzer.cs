using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlAnalyzer
    {
        private readonly LinkedList<ISelectionSymbol> Symbols = new LinkedList<ISelectionSymbol>();
        private readonly List<StringBuilder> Builders = new List<StringBuilder>();
        private readonly HashSet<char> SpecialSymbols = new HashSet<char> {'_'};

        public string AnalyzeLine(string line)
        {
            //throw new Exception(line);
            var currentBuilder = new StringBuilder();
            Console.WriteLine(line);

            for (int i = 0; i < line.Length; i++)
            {
                var symbol = line[i];
                //Console.WriteLine(symbol);
                switch (symbol)
                {
                    case '\\':
                        if (SpecialSymbols.Contains(line[i + 1]) || line[i + 1] == '\\')
                        {
                            currentBuilder.Append(line[i + 1]);
                            i++;
                        }
                        else
                        {
                            currentBuilder.Append(line[i]);
                        }
                        break;

                    case '_':
                        ISelectionSymbol newSpecialSymbol;
                        if (i != line.Length-1 && line[i + 1] == '_')
                        {
                            if (i<= line.Length -4 && line[i + 2] == '_' && line[i + 3] == '_')
                            {
                                currentBuilder.Append("____");
                                i += 3;
                                break;
                            }

                            newSpecialSymbol = new Bold();
                            if (i + 2 != line.Length && line[i + 2] != ' ')
                                newSpecialSymbol.IsPossibleStartElement = true;
                            if (i != 0 && line[i - 1] != ' ')
                                newSpecialSymbol.IsPossibleEndElement = true;

                            i++;

                            Symbols.AddLast(newSpecialSymbol);

                            var currentSpecialSymbol = Symbols.Last.Previous;

                            Bold opener = null;

                            var counterOfSingleUnderlinings = 0;

                            while (currentSpecialSymbol != null)
                            {
                                if (currentSpecialSymbol.Value is Italics)
                                    counterOfSingleUnderlinings++;

                                if (opener == null)
                                {
                                    if (!currentSpecialSymbol.Value.IsClosed &&
                                        currentSpecialSymbol.Value is Bold)
                                    { 
                                        opener = currentSpecialSymbol.Value as Bold; 
                                        currentSpecialSymbol = currentSpecialSymbol.Previous; 
                                        if (currentSpecialSymbol == null)
                                        {
                                            if (counterOfSingleUnderlinings%2 == 0)
                                                MakePair(opener);
                                            break;
                                        }
                                        continue;
                                    }

                                    if (currentSpecialSymbol.Value.IsClosed &&
                                        currentSpecialSymbol.Value is Bold) 
                                        break;
                                }

                                else
                                {
                                    var currentValue = currentSpecialSymbol.Value;
                                    if (currentValue is Bold ||
                                        currentValue is Italics && currentValue.IsClosed)
                                    {
                                        if (counterOfSingleUnderlinings%2 == 0)
                                            MakePair(opener);
                                        break;
                                    }
                                    if (currentValue is Italics && !currentValue.IsClosed)
                                        break;
                                }

                                currentSpecialSymbol = currentSpecialSymbol.Previous;
                            }
                        }
                        else if (i<line.Length-1 && char.IsDigit(line[i + 1]) || i>0 && char.IsDigit(line[i-1]))
                        {
                            currentBuilder.Append(symbol);
                        }
                        else
                        {
                            newSpecialSymbol = new Italics();
                            if (i + 1 != line.Length && line[i + 1] != ' ')
                                newSpecialSymbol.IsPossibleStartElement = true;
                            if (i != 0 && line[i - 1] != ' ')
                                newSpecialSymbol.IsPossibleEndElement = true;

                            Symbols.AddLast(newSpecialSymbol);

                            var currentSpecialSymbol = Symbols.Last.Previous;

                            var counterOfDoubleUnderlinings = 0;

                            while (currentSpecialSymbol !=null)
                            {
                                if (currentSpecialSymbol.Value is Bold)
                                    counterOfDoubleUnderlinings++;

                                if (!currentSpecialSymbol.Value.IsClosed &&
                                     currentSpecialSymbol.Value is Italics)
                                {
                                    if (counterOfDoubleUnderlinings%2 == 0) 
                                        MakePair(currentSpecialSymbol.Value);
                                    break;
                                }
                                
                                if (currentSpecialSymbol.Value.IsClosed &&
                                         currentSpecialSymbol.Value is Italics)
                                    break;
                                
                                currentSpecialSymbol = currentSpecialSymbol.Previous;
                            }
                        }

                        
                        Builders.Add(currentBuilder);
                        currentBuilder = new StringBuilder();
                        break;

                    case ' ':
                        currentBuilder.Append(symbol);
                        break;

                    default:
                        currentBuilder.Append(symbol);
                        break;
                }

            }
            Builders.Add(currentBuilder);

            var result = new StringBuilder();
            for (int i = 0; i < Builders.Count; i++)
            {
                result.Append(Builders[i]);
                if (Symbols.Count != 0)
                {
                    result.Append(Symbols.First.Value.HtmlTagAnalog);
                    Symbols.RemoveFirst();
                }

            }

            Console.WriteLine(result.ToString());
            return result.ToString();
            throw new NotImplementedException();
        }

        private void MakePair<T>(T opener) where T: ISelectionSymbol
        {
            var ender = Symbols.Last.Value;
            if (opener.IsPossibleStartElement && ender.IsPossibleEndElement)
            {
                opener.IsClosed = true;
                opener.IsStartTag = true;
                Symbols.Last.Value.IsClosed = true;
            }
        }
    }
}