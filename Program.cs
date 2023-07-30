using System;
using System.Data;
using HabitLogger;
using Microsoft.Data.Sqlite;

namespace ConsoleHabbitLogger;

class Program
{
	static void Main(string[] args)
	{
		DatabaseManager dbManager = new DatabaseManager("habitlogger.db");
		dbManager.CreateHabit("Drink coffee", 3);
		dbManager.CreateHabit("Drink balls", 2);
		dbManager.UpdateHabit(2, "Drink water", DatabaseManager.UpdateOptions.Name);
		dbManager.UpdateHabit(2, "50", DatabaseManager.UpdateOptions.Quantity);
		dbManager.UpdateHabit(2, DateTime.UtcNow.AddDays(3).ToShortDateString(), DatabaseManager.UpdateOptions.Date);
		dbManager.ReadHabit(DatabaseManager.ReadOptions.All);
	}
}