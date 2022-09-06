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
                    case null:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case ConsoleColor:
                        Console.ForegroundColor = (ConsoleColor)o;
                        break;
                    case string:
                        Console.Write(o.ToString());
                        break;
                    default:
                        Console.WriteLine("The fuck did you put in my writer");
                        break;
                }
            }
        }
    }

}
