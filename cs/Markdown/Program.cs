using Markdown;

var text = @"# _Billie Jean_ is ___ not my __lover__\n
             She's #just# a __girl__ # ____ who __cl_aim_s _that_ I am the one__\n
             # But the __kid \_\_is\_\_ not my son__\\n
             # She __says _I am__ the_ one, but the _kid is __not__ my son_";

//text = @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ ";

Console.WriteLine(MD.Render(text));