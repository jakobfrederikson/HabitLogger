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
            switch (mainMenu())
            {
                case "0":
                    RunApp = false;
                    break;
                case "1":
                    createHabit();
                    break;
                case "2":
                    editHabit();
                    break;
                case "3":
                    deleteHabit();
                    break;
                case "4":
					habitLogMenu();
					break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                default:
                    break;
            }
		}
    }

    private void Title()
    {
        Console.Clear();
		Console.WriteLine("Habit Logger");
		Console.WriteLine("------------\n");
	}

    private string mainMenu()
    {
        Title();

        if (dbManager.Habits.Empty)
        {
            Console.WriteLine("You haven't created any habits yet.\n");
            Console.WriteLine("[1] Create a new habit\n");
        }
        else
        {
            dbManager.Habits.Read();
			Console.WriteLine("[1] Create a new habit");
            Console.WriteLine("[2] Edit a habit");
            Console.WriteLine("[3] Delete a habit\n");
            Console.WriteLine("[4] Create a log");
            Console.WriteLine("[5] View logs");
            Console.WriteLine("[6] Edit a log");
            Console.WriteLine("[7] Delete a log");
        }
		Console.WriteLine("\n[0] Exit");
		Console.Write("\nSelect an option: ");
        return Console.ReadLine();
	}

    private void createHabit()
    {
        Title();
        string? habitName = ConsoleHelper.GetValidString("Enter name of the habit to create: ");
        dbManager.Habits.Create(habitName);
    }

    private void editHabit()
    {
        Title();
        dbManager.Habits.Read();
        string? response = ConsoleHelper.GetValidString("Write the id of the habit you want to edit: ");
        int id = 0;
		int.TryParse(response, out id);

		ConsoleHelper.CreateMenu($"Edit habit #{response}", new string[] { "Name", "Date" }, "Back");
		switch (ConsoleHelper.GetValidString("Select an option: "))
        {
            case "1":
                string nameString = ConsoleHelper.GetValidString("Enter new name: ");
				dbManager.Habits.Update(id, nameString, UpdateOptions.Hname);
                break;
            case "2":
				string updateString = ConsoleHelper.GetValidString("Enter new Date (d/mm/yyyy): ");
				dbManager.Habits.Update(id, updateString, UpdateOptions.Date);
                break;
            case "0":
                return;
            default:
                break;
        }
    }

    private void deleteHabit()
    {
        Title();
        dbManager.Habits.Read();
        int id = ConsoleHelper.GetInt("Enter the id of the habit you wish to delete: ");

        if (ConsoleHelper.Confirm("Are you sure you want to delete this habit?"))
        {
            Console.WriteLine($"Deleting habit of ID: {id}");
            dbManager.Habits.Delete(id);
            Console.Write("Press any key to continue. . .");
            Console.ReadKey(false);
		}
    }

    private void habitLogMenu()
    {
		Title();
		if (dbManager.HabitsTracker.Empty)
        {
            Console.WriteLine("You haven't logged any of your habits yet.\n");
            Console.WriteLine("[1] Create a new log");
            
        }
        else
        {
            dbManager.HabitsTracker.Read();
			Console.WriteLine("[1] Create a new log");
			Console.WriteLine("[2] Edit a log");
			Console.WriteLine("[3] Delete a log");
            Console.WriteLine("[4] View logs for a specific habit");
		}
        Console.WriteLine("\n[0] Back to main menu");
        
        Console.ReadLine();
	}
}
