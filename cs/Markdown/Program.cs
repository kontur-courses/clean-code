namespace Markdown
{
    class Program
    {
        public static void Main()
        {
            var md = new Markdown();
            var html = md.Render("__Жир_Курсив_Жир__");
        }
    }
}
