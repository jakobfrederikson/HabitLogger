using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ConsoleTables;

namespace HabitLogger.src.Database;

public class DatabaseManager
{
    private static DatabaseManager? instance;
    public static DatabaseManager Instance { 
        get 
        {
            if (instance == null)
                instance = new DatabaseManager();

            return instance;
        }
    }
    public HabitsTable Habits { get; } = new HabitsTable();
    public HabitsTrackerTable HabitsTracker { get; } = new HabitsTrackerTable();
}
