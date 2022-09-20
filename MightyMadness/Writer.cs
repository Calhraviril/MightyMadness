namespace MightyMadness
{
    public class Writer
    {
        public void WriteColored(params object[] oo)
        {
            foreach (var o in oo)
            {
                switch (o)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 1:
                        Console.WriteLine();
                        break;
                    case ConsoleColor:
                        Console.ForegroundColor = (ConsoleColor)o;
                        break;
                    case string:
                        Console.Write(o.ToString());
                        break;
                    default:
                        Console.WriteLine("WriteColored does not understand a given input");
                        break;
                }
            }
        }
        public string WriteList(object[] oo)
        {
            int o = 0;
            string returnable = "";
            foreach (var item in oo)
            {
                o++;
                if (item is Skill)
                {
                    Skill fitem = (Skill)item;
                    returnable = returnable + " " + o + ". " + fitem.Name;
                }
                else if (item is Unit)
                {
                    Unit fitem = (Unit)item;
                    returnable = returnable + " " + o + ". " + fitem.Name;
                }
                else
                {
                    returnable = returnable + " " + o + ". Unknown classification";
                }
            }
            return returnable;
        }
        public void Flash(ConsoleColor flashed, int duration)
        {
            Console.BackgroundColor = flashed;
            Console.Clear();
            Thread.Sleep(duration);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }
    }
}
