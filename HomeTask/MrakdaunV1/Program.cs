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

            
            
            foreach (var tuple in cases)
            {
                // для начала введем массив состояния для каждого символа.
                // значения в этом массиве будут определять, нужно ли символ удалять или
                // оставлять, а также покажут, к какому тегу относится символ
                CharState[] charStates = new CharState[tuple.Data.Length];
                
                List<TokenPart> tokenParts = new();
                Console.WriteLine($"----------\nCase \"{tuple.CaseName}\": \"{tuple.Data}\"");
                
                for (var i = 0; i < tuple.Data.Length; i++)
                {
                    // ищем землю
                    if (tuple.Data[i] != '_')
                        continue;

                    // тройные земли пока игнорируются
                    if (i <= tuple.Data.Length - 3
                        && tuple.Data[i] == '_'
                        && tuple.Data[i + 1] == '_'
                        && tuple.Data[i + 2] == '_')
                    {
                        i += 2;
                        continue;
                    }
                    
                    // создаем кусочек токена, он хранит инфу о том, какой это тег + добавочная инфа
                    var tokenPart = new TokenPart(tuple.Data, i);
                    
                    // у жира две земли
                    if (tokenPart.Type == TokenPartType.Bold)
                        i++;
                    
                    tokenParts.Add(tokenPart);
                }


                // теперь, когда мы распарсили куски тегов, можно парсить их самих
                // начнем с парсинга курсива, потому что так легче будет с взаимодействием
                // с жирным тегом
                List<TokenPart> listOfItalicTokenParts = tokenParts
                    .Where(p => p.Type == TokenPartType.Italic)
                    .ToList();

                for (int i = 0; i < listOfItalicTokenParts.Count; i++)
                {
                    // на одном теге далеко не уедешь, поэтому просто запомним, что он есть
                    if (i == 0)
                        continue;
                    
                    // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                    if ((charStates[listOfItalicTokenParts[i - 1].Index] & CharState.Special) == CharState.Special)
                        continue;
                    
                    // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                    var sliceHasDelimeters =
                        tuple.Data.ContainsDelimeters(
                            listOfItalicTokenParts[i - 1].Index,
                            listOfItalicTokenParts[i].Index);

                    
                    var currnetTokPosType = listOfItalicTokenParts[i].PositionType;
                    var prevPosType = listOfItalicTokenParts[i - 1].PositionType;
                    
                    // 4 проверки - 4 условия существования валидной пары тегов
                    // 1 - тег до и после слов, хоть убейся, но будет валиден всегда
                    // 2, 3 и 4 случаи так или иначе задействуют часть в слове, а тут нужно учесть,
                    // что это не должно быть число, а также слово должно быть одно, без разделителей (пробел)
                    if ((currnetTokPosType == TokenPartPositionType.AfterWord
                         && prevPosType == TokenPartPositionType.BeforeWord)
                        
                        || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                            && prevPosType == TokenPartPositionType.BeforeWord
                            && !sliceHasDelimeters
                            && !listOfItalicTokenParts[i].IsInsideTheNumber)
                        
                        || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                            && prevPosType == TokenPartPositionType.InsideTheWord
                            && !sliceHasDelimeters
                            && !listOfItalicTokenParts[i].IsInsideTheNumber
                            && !listOfItalicTokenParts[i - 1].IsInsideTheNumber)
                        
                        || (currnetTokPosType == TokenPartPositionType.AfterWord
                            && prevPosType == TokenPartPositionType.InsideTheWord
                            && !sliceHasDelimeters
                            && !listOfItalicTokenParts[i - 1].IsInsideTheNumber))
                    {
                        
                        // помечаем все символы флагами курсива + спецсимволы также пометим
                        for (var c = 0;
                            c < listOfItalicTokenParts[i].Index - listOfItalicTokenParts[i - 1].Index + 1;
                            c++)
                            charStates[c + listOfItalicTokenParts[i - 1].Index] |= CharState.Italic;

                        charStates[listOfItalicTokenParts[i].Index] |= CharState.Special;
                        charStates[listOfItalicTokenParts[i - 1].Index] |= CharState.Special;
                    }
                }
                
                
                
                
                List<TokenPart> listOfBoldTokenParts = tokenParts
                    .Where(p => p.Type == TokenPartType.Bold)
                    .ToList();
                
                for (int i = 0; i < listOfBoldTokenParts.Count; i++)
                {
                    // на одном теге далеко не уедешь, поэтому просто запомним, что он есть
                    if (i == 0)
                        continue;
                    
                    // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                    if ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Special) == CharState.Special)
                        continue;
                    
                    // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                    var sliceHasDelimeters =
                        tuple.Data.ContainsDelimeters(
                            listOfBoldTokenParts[i - 1].Index,
                            listOfBoldTokenParts[i].Index);

                    var currnetTokPosType = listOfBoldTokenParts[i].PositionType;
                    var prevPosType = listOfBoldTokenParts[i - 1].PositionType;

                    
                    
                    // Абсолютно конченый IF
                    // жир имеет шансы на валидность, если:
                    // 1) в нем нет курсива
                    // 2) курсив есть, но по бокам курсива точно нет (нет коллизий)
                    if (
                        charStates.Skip(listOfBoldTokenParts[i-1].Index)
                            .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i-1].Index)
                            .All(cs => (cs & CharState.Italic) != CharState.Italic)
                        
                        ||(charStates.Skip(listOfBoldTokenParts[i-1].Index)
                            .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i-1].Index)
                            .Any(cs => (cs & CharState.Italic) == CharState.Italic)
                        && (charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) !=
                        CharState.Italic
                        && (charStates[listOfBoldTokenParts[i].Index] & CharState.Italic) !=
                        CharState.Italic))
                    {
                        // 4 проверки - 4 условия существования валидной пары тегов
                        // 1 - тег до и после слов, хоть убейся, но будет валиден всегда
                        // 2, 3 и 4 случаи так или иначе задействуют часть в слове, а тут нужно учесть,
                        // что это не должно быть число, а также слово должно быть одно, без разделителей (пробел)
                        if ((currnetTokPosType == TokenPartPositionType.AfterWord
                             && prevPosType == TokenPartPositionType.BeforeWord)

                            || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                                && prevPosType == TokenPartPositionType.BeforeWord
                                && !sliceHasDelimeters
                                && !listOfBoldTokenParts[i].IsInsideTheNumber)

                            || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                                && prevPosType == TokenPartPositionType.InsideTheWord
                                && !sliceHasDelimeters
                                && !listOfBoldTokenParts[i].IsInsideTheNumber
                                && !listOfBoldTokenParts[i - 1].IsInsideTheNumber)

                            || (currnetTokPosType == TokenPartPositionType.AfterWord
                                && prevPosType == TokenPartPositionType.InsideTheWord
                                && !sliceHasDelimeters
                                && !listOfBoldTokenParts[i - 1].IsInsideTheNumber))
                        {
                            for (var c = 0;
                                c < listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i - 1].Index + 2;
                                c++)
                                charStates[c + listOfBoldTokenParts[i - 1].Index] |= CharState.Bold;
                            
                            charStates[listOfBoldTokenParts[i].Index] |= CharState.Special;
                            charStates[listOfBoldTokenParts[i].Index+1] |= CharState.Special;
                            charStates[listOfBoldTokenParts[i - 1].Index] |= CharState.Special;
                            charStates[listOfBoldTokenParts[i - 1].Index+1] |= CharState.Special;
                        }
                    }
                    // если же коллизии есть, то надо игнорить жир + убрать курсивное выделение
                    else
                    {
                        // если вообще все символы курсивные, то мы имеем дело с жиром внутри курсива, в целом
                        // это валидно, просто игнорим
                        if (
                            charStates.Skip(listOfBoldTokenParts[i-1].Index)
                                .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i-1].Index)
                                .All(cs => (cs & CharState.Italic) == CharState.Italic))
                            continue;
                        
                        /*
                         * Сейчас сложная ситуация - в жирном есть курсив и надо понять,
                         * где пересечения + обнулить его
                         */

                        var numOfCollisions = 0;
                        numOfCollisions += ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) ==
                                            CharState.Italic) ? 1 : 0;
                        numOfCollisions += ((charStates[listOfBoldTokenParts[i].Index] & CharState.Italic) ==
                                            CharState.Italic) ? 1 : 0;

                        while (numOfCollisions != 0)
                        {
                            var isCollisionFromLeft = ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) ==
                                                       CharState.Italic);
                        
                            // Произошло пересечение с левой границей
                            // Запускаем обнулялку влево и вправо
                            var currentIndex = isCollisionFromLeft ? listOfBoldTokenParts[i - 1].Index : listOfBoldTokenParts[i].Index;
                        
                            while ((charStates[currentIndex] & CharState.Special) != CharState.Special)
                            {
                                charStates[currentIndex--] &= ~CharState.Italic;
                            }
                            charStates[currentIndex] &= ~(CharState.Italic | CharState.Special);
                        
                            currentIndex = isCollisionFromLeft ? listOfBoldTokenParts[i - 1].Index : listOfBoldTokenParts[i].Index;
                            while ((charStates[currentIndex] & CharState.Special) != CharState.Special)
                            {
                                charStates[currentIndex++] &= ~CharState.Italic;
                            }
                            charStates[currentIndex] &= ~(CharState.Italic | CharState.Special);

                            numOfCollisions--;
                        }
                    }
                }


                
                
                Console.WriteLine("Char states:");
                foreach (var state in charStates)
                {
                    //var name = Enum.GetName(state);
                    //if (name is null)
                    //{
                    var name = "";
                    if ((state & CharState.Bold) == CharState.Bold)
                        name += "B";
                    if ((state & CharState.Italic) == CharState.Italic)
                        name += "I";
                    if ((state & CharState.Special) == CharState.Special)
                        name += "S";
                    if (name == "")
                        name = "_";
                    //}
                    Console.Write(name + " ");
                }
                
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}