using System;
using HabitLogger;
using HabitLogger.Database;

namespace ConsoleHabbitLogger;

class Program
{
	static void Main(string[] args)
	{
		DatabaseManager db = new DatabaseManager("habitlogger.db", 
												 new HabitsTable(),
												 new HabitsTrackerTable());
		ConsoleManager consoleManager = new ConsoleManager(db);

		consoleManager.StartApp();
	}
}