using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src;

public class ConsoleHelper
{
	public static void CreateMenu(string menuTitle, string[] menuOptions, string exitOption = "Exit")
	{
        Console.WriteLine(menuTitle);
        for (int i = 0; i < menuOptions.Length; i++)
		{
            Console.WriteLine($"[{i + 1}] {menuOptions[i]}");
        }
		Console.WriteLine($"\n[0] {exitOption}");
	}

	public static string GetValidString(string message)
	{
		Console.Write(message);

		string response = Console.ReadLine();

		if (response == "")
		{
			while (response == "")
			{
                Console.Write("Please enter a non-empty string: ");
				response = Console.ReadLine();
            }
		}

		return response;
	}

	public static int GetInt(string message)
	{
		int number = 0;

        Console.Write(message);
		while(!int.TryParse(Console.ReadLine(), out number))
		{
            Console.WriteLine($"{number} is not a number. Enter a valid id: ");
        }

		return number;
    }

	public static bool Confirm(string message)
	{
		ConsoleKey response;

		do
		{
			Console.Write($"{message} [y/n] ");
			response = Console.ReadKey(false).Key;
			if (response != ConsoleKey.Enter)
				Console.WriteLine();
		} while (response != ConsoleKey.Y && response != ConsoleKey.N);

		return response == ConsoleKey.Y;
    }
}
