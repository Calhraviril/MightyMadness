namespace MightyMadness
{
    public class Writer
    {
        // Converts a given line of instructions into a more graphical WriteLine
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
                        Console.WriteLine("WriteColored does not understand a given input, ye stoopid hark");
                        break;
                }
            }
        }

        // Converts list of object names into a string
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

        public string ListJsonUnit()
        {
            string listed = "";
            int orderPosition = 0;
            while (true)
            {
                try
                {
                    listed = listed + " " + (orderPosition + 1) + ". " + DH.allyHandler.Units[orderPosition].Name;
                    orderPosition++;
                }
                catch
                {
                    break;
                }
            }
            return listed;
        }

        // Flashes the screen with ConsoleColor for Duration
        public void Flash(ConsoleColor flashed, int duration)
        {
            Console.BackgroundColor = flashed;
            Console.Clear();
            Thread.Sleep(duration);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }
        
        // Simply writes a tutorial for some commands
        public void Tutor()
        {
            WriteColored(ConsoleColor.White, "Once all actions are chosen, press any key without an already set use to complete the turn", 1, "Press CTRL + Z to revert a turn, despite it being stupid", 1, "Press CTRL + Q to revert a choice", 1, 0);
        }
    }
}
