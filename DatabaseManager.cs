using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ConsoleTables;

namespace HabitLogger;

public class DatabaseManager
{
	public IHabitLoggerTable Habits { get; }
	public IHabitLoggerTable HabitsTracker { get; }

	public enum HabitUpdateOptions { Name, Date }

	public DatabaseManager(string _Filename, HabitsTable _Habits, HabitsTrackerTable _HabitsTracker)
	{
		Habits = _Habits;
		Habits.Filename = _Filename;
		Habits.CreateTableIfNotExists();

		HabitsTracker = _HabitsTracker;
		HabitsTracker.Filename = _Filename;		
		HabitsTracker.CreateTableIfNotExists();
	}
}
