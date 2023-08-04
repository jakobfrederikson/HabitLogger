using HabitLogger.src.DeclarativeConsoleMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src;

public class ConsoleHelper
{
	public static string GetValidString(string message)
	{
        Console.Write(message);
        string response;
        do
		{
			response = Console.ReadLine();
		} while (string.IsNullOrEmpty(response));

		return response;
	}

	public static int GetInt(string message)
	{
		int number;

        Console.Write(message);
		while(!int.TryParse(Console.ReadLine(), out number))
		{
            Console.Write($"{number} is not valid. Try again: ");
        }

		return number;
    }

    public static int GetInt(string message, int maxValue)
    {
        int number;

        Console.Write(message);
        while (!int.TryParse(Console.ReadLine(), out number) || number > maxValue || number < 0)
        {
            Console.Write($"{number} is not valid. Try again: ");
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
