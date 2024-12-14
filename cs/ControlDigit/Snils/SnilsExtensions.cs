using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Internal.Execution;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number){
            var dict = NumberDecompotion(number);
            var M = SummDecompotion(dict);
            var result = MakeControlDigit(M);
            return result;
        }

        private static Dictionary<int, int> NumberDecompotion(this long number){
            var counter = 1;
            var dict = new Dictionary<int, int>();
            while(number / 10 >= 1){
                dict.Add(counter, (int)number % 10);
                counter++;
                number = number / 10;
            }
            dict.Add(counter, (int)number % 10);
            return dict;
        }

        private static int SummDecompotion(Dictionary<int, int> dict){
            var summ = 0;
            foreach(var pair in dict){
                summ += pair.Key * pair.Value;
            }
            return summ;
        }

        private static IEnumerable<int> MakeFactorsSnils(int length){
            return Enumerable.Range(1, length);
        }

        private static IEnumerable<int> MakeFactorsUpc(int length){
            return Enumerable.Range(1, length);
        }

        private static int CountControlDigit(IEnumerable<int> values, Func<int, IEnumerable<int>> evaluateFactors, int length){
            var factors = evaluateFactors(length);
            var summ = 0;
            //for(int i = 0;i < length;i++){
                //summ += factors[i] * values[i];
            //}
            var ggygu = factors.Zip(values)
        }

        private static int MakeControlDigit(int summ){
            if (summ > 101)
                summ = summ % 101;
            if (summ < 100)
                return summ;
            else
                return 0;
        }
    }
}
