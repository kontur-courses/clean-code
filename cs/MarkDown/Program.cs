using MarkDown;

var tests = new[]
{
    "_a_", "__a__", "___a___", "____",
    "a_b_c", "С_трок_а", "ра_зных сл_овах",
    "_a1b_", "__a1b__", "_12_3",
    "__a _b_ c__", "_a __b__ c_",
    "__a_b__c_", "_a_b_c_",
    "\\_a\\_", "\\\\_a_", "сим\\волы \\должны остаться.\\",
    "# Заголовок __с _разными_ символами__",
};
foreach (var t in tests)
{
    Console.WriteLine($"IN : {t}");
    Console.WriteLine($"OUT: {Md.Render(t)}");
    Console.WriteLine();
}
