using MightyMadness;
using System;

Console.BackgroundColor = ConsoleColor.Red;
Console.ForegroundColor = ConsoleColor.Black;

Battlecontrol mainBattle = new();

mainBattle.ManifestArmy(3);

mainBattle.ListArmies();

while (true)
{
    Console.WriteLine("Your turn: ");
    int attacker = Convert.ToInt32(Console.ReadLine()) - 1;
    int attacked = Convert.ToInt32(Console.ReadLine()) - 1;
    mainBattle.BattleScenario(attacker, attacked);
    
    if (mainBattle.DeadMansClaw())
    {
        break;
    }
}

