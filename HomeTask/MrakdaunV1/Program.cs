using System;
using System.Collections.Generic;
using System.Linq;
using MrakdaunV1.Enums;


namespace MrakdaunV1
{
    public class Program
    {
        public static void Main()
        {
            var engine = new MrakdaunEngine();
            Console.WriteLine(engine.GetParsedText("__aa _bb_ _cc_ aa__"));
            Environment.Exit(0);
            
            
            List<(string CaseName, string Data)> cases = new()
            {
                ("выделить все слово","__aaaa__"),
                ("выделить все слово","_aaaa_"),
                ("выделить часть слова","_aa_aa"),
                ("ничего (в конце должен быть пробел)","_aaa _aaa"),
                ("ничего (за началом должен быть НЕ пробел)","aaa_ aaa_ aaa"),
                ("ничего не делать","_aaa aa_aa"),
                ("выделить два слова","_aaa aa_ aa"),
                ("выделить часть второго слова","_aaa aa_aa_"),
                ("выделить первую часть слова","_aa_aa_"),
                ("ничего","__aa_"),
                //("ничего","___aa___"), // пизда
                //("ничего","__aa___"), // пизда
                ("выделение","_aaa1_"),
                ("ничего","aaa_11_111a"),
                ("ничего","_1111_111"),
                ("выделение","_111_"),
                ("bb НЕ жирный, остальное курсив","_aa __bb__ aa_"),
                ("bb курсив, и еще все жирное","__aa _bb_ aa__"),
                ("весь текст жирный с элементами курсива","__aa _bb_ _cc_ aa__"),
                ("bb будут курсив, а жира нигде не будет, сс не будет выделен","__aa _bb_ aa _c aa__ c_"),
                ("вообще нет выделения","_b __aa b_ _b aa__ b_"),
                ("ничего (пересечение)","__aa_aa__aa_"),
                ("ничего (пересечение) 2","__aaa _aa__ a aa_"),
            };
        }
    }
}