using ConsoleTables;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;

public class HabitsTable : IHabitLoggerTable
{
    public string? Filename { get; set; }

    public void CreateTableIfNotExists()
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText =
                    @" 
						CREATE TABLE IF NOT EXISTS Habits (
							HabitId		INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
							HabitName	TEXT NOT NULL,
							Date		TEXT NOT NULL
						);
					";

                TableHelper.TryExecuteNonQuery(connection, command);
            }
        }
    }

    /// <summary>
    /// Create a habit to be inserted in the Habits table.
    /// </summary>
    /// <param name="habitName">The name of the habit.</param>
    public void Create(string habitName = "default", int habitId = 0, int habitQuantity = 0)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText =
                    @"
						INSERT INTO Habits (HabitName, Date) 
						VALUES ($name,$date);
					";
                command.Parameters.AddWithValue("$name", habitName);
                command.Parameters.AddWithValue("$date", DateTime.Now.ToShortDateString());

                TableHelper.TryExecuteNonQuery(connection, command);
            }
        }
    }

    /// <summary>
    /// View all entries within the Habits table.
    /// </summary>
    public void Read()
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                var table = new ConsoleTable("Id", "Habit", "Date Started");
                command.CommandText = "SELECT * FROM Habits";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetString(0);
                        var name = reader.GetString(1);
                        var date = reader.GetString(2);
                        table.AddRow(id, name, date);
                    }

                    table.Write();
                }
            }
        }
    }

    /// <summary>
    /// Update the name or date of a habit in the Habits table.
    /// </summary>
    /// <param name="habitId">The ID of the habit.</param>
    /// <param name="updateString">The string to replace the old data.</param>
    /// <param name="updateOption">Supply the option to update the name or date.</param>
    public void Update(int habitId, string updateString, UpdateOptions updateOption)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                switch (updateOption)
                {
                    case UpdateOptions.Hname:
                        command.CommandText =
                            @"
								UPDATE Habits
								SET HabitName = $value
								WHERE HabitId = $habitId;
							";
                        break;
                    case UpdateOptions.Date:
                        command.CommandText =
                            @"
								UPDATE Habits
								SET Date = $value
								WHERE HabitId = $habitId;
							";
                        break;
                    default:
                        break;
                }

                command.Parameters.AddWithValue("$value", updateString);
                command.Parameters.AddWithValue("$habitId", habitId);

                TableHelper.TryExecuteNonQuery(connection, command);
            }
        }
    }

    /// <summary>
    /// Delete a habit from the Habits table.
    /// </summary>
    /// <param name="habitId">The id of the habit to delete.</param>
    public void Delete(int habitId)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                            @"
								DELETE FROM Habits
								WHERE HabitId = $habitId;
							";
                command.Parameters.AddWithValue("$habitId", habitId);

                TableHelper.TryExecuteNonQuery(connection, command);
            }
        }
    }
}
