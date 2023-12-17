using Markdown;

var text = @"# _Billie Jean_ is ___ not my __lover__\n
             She's #just# a __girl__ # ____ who __cl_aim_s _that_ I am the one__\n
             # But the __kid \_\_is\_\_ no_t m_y son__ \\n
             # She __says _I am__ the_ one, __but__ the _kid is __not__ my son_";

Console.WriteLine(MD.Render(text));