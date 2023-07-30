using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;

public class TableHelper
{
    public static void TryExecuteNonQuery(SqliteConnection connection, SqliteCommand command)
    {
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Could not create habit table! Details: {e.Message}");
        }
        finally
        {
            command.Dispose();
            connection.Dispose();
        }
    }
}
