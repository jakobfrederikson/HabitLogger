using System;
using System.Data;
using HabitLogger;
using Microsoft.Data.Sqlite;

namespace ConsoleHabbitLogger;

class Program
{
	static void Main(string[] args)
	{
		DatabaseManager dbManager = new DatabaseManager("habitlogger.db", 
														new HabitsTable(),
														new HabitsTrackerTable());
		//dbManager.Habits.Create("Drink Water");
		//dbManager.HabitsTracker.Create("", 1, 3);
		dbManager.Habits.Read();
		dbManager.HabitsTracker.Read();
	}
}