using System;
using System.Data;
using HabitLogger;
using Microsoft.Data.Sqlite;

namespace ConsoleHabbitLogger;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine($"Current date: {DateTime.Now.ToShortDateString()}");
		DatabaseManager dbManager = new DatabaseManager("habitlogger.db", 
														new HabitsTable(),
														new HabitsTrackerTable());
		//dbManager.Habits.Create("Drink Water");
		//dbManager.HabitsTracker.Create("", 1, 2);
		dbManager.Habits.Read();
		dbManager.HabitsTracker.Read();
	}
}