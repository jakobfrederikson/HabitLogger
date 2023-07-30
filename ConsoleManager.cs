using HabitLogger.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger;

public class ConsoleManager
{
    private DatabaseManager dbManager;

    public ConsoleManager(DatabaseManager dbManager)
    {
        this.dbManager = dbManager;
    }

    public void StartApp()
    {

        string response = Console.ReadLine();

        switch (response)
        {
            case "a":
                break;
            default:
                break;
        }
    }
}
