using HabitLogger.src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src;

public class ConsoleManager
{
    private DatabaseManager dbManager;

    private bool RunApp { get; set; } = true;

    public ConsoleManager(DatabaseManager dbManager)
    {
        this.dbManager = dbManager;
    }

    public void StartApp()
    {
        while (RunApp)
        {
			MainMenu();

            string response = Console.ReadLine();

            switch (response)
            {
                case "0":
                    RunApp = false;
                    break;
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    HabitLogMenu();
                    break;
                default:
                    break;
            }

            Console.Clear();
		}
    }

    private void Title()
    {
		Console.WriteLine("Habit Logger");
		Console.WriteLine("------------\n");
	}

    private void MainMenu()
    {
        Title();

        /* 
         TODO:
        If Habits table is EMPTY:
            cw."You have no habits logged. Enter '1' to create a new habit."
        else:
            Display all habits
            cw."Select a habit: "

        If Habit selected:
            cw."HABIT NAME"
            Display all logs for this habit
            cw."View habit options"
            cw."View log options"
            cw."Back"        
         */

        if (dbManager.Habits.Empty)
            Console.WriteLine("Habits is empty");
        else
            Console.WriteLine("Habits has data");

		if (dbManager.HabitsTracker.Empty)
			Console.WriteLine("HabitsTracker is empty");
		else
			Console.WriteLine("HabitsTracker has data");

		Console.WriteLine("Select an option\n");

        Console.WriteLine("[1] Create a new habit");
        Console.WriteLine("[2] Log a habit");
        Console.WriteLine("[3] ");

        Console.WriteLine("[0] To exit");
		Console.Write("What option do you choose? ");
	}

    private void HabitLogMenu()
    {
        Title();
		Console.WriteLine("Select an option");
		Console.WriteLine("[0] To exit");
		Console.WriteLine("[1] Create a habit log");
		Console.WriteLine("[2] View all habits");
		Console.WriteLine("[3] Edit a habit");
		Console.WriteLine("[4] Delete a habit");
		Console.WriteLine("[5] Create a habit log");
		Console.Write("What option do you choose? ");
	}
}
