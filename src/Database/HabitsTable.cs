using ConsoleTables;
using HabitLogger.src.DeclarativeConsoleMenu;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;


public enum HabitsUpdateOptions { Date, Name }
public class HabitsTable
{
    public HabitsTable()
    {
        Filename = "habitlogger.db";
        CreateTableIfNotExists();
    }

    public string? Filename { get; }
    public bool Empty => isEmpty();

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
    /// Returns true if the table is empty.
    /// </summary>
    /// <returns></returns>
	private bool isEmpty()
    {
		using (var connection = new SqliteConnection($"Data Source={Filename}"))
		{
			using (var command = connection.CreateCommand())
			{                
				connection.Open();
				command.CommandText =
					@" 
						SELECT * FROM Habits;
					";

				using var reader = command.ExecuteReader();
				return reader.HasRows ? false : true;
			}
		}
    }

	/// <summary>
	/// Create a habit to be inserted in the Habits table.
	/// </summary>
	public void Create()
    {
        string habitName = ConsoleHelper.GetValidString("Enter the habit name: ");
        if (ConsoleHelper.Confirm("Confirm habit creation?"))
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
            Console.Write($"New habit {habitName.ToUpper()} created.");
            Console.ReadKey(false);
        } 
        else
        {
            Console.Write($"Cancelled habit creation.");
            Console.ReadKey(false);
        }
    }

    /// <summary>
    /// View all entries within the Habits table.
    /// </summary>
    public void Read(bool waitForUserResponse)
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
            if (waitForUserResponse)
            {
                Console.Write("Press any key to continue. . .");
                Console.ReadKey(false);
            }            
        }
    }

    /// <summary>
    /// Update the name or date of a habit in the Habits table.
    /// </summary>
    public void Update()
    {
        Read(false);
        int habitId = ConsoleHelper.GetInt("Enter the id of the entry you'd like to change: ");
        Console.WriteLine("[0] Date (d-mm-yyyy)");
        Console.WriteLine("[1] Name");
        HabitsUpdateOptions updateOption = (HabitsUpdateOptions)ConsoleHelper.GetInt("Choose what you would like to update: ", 1);
        string updateString = ConsoleHelper.GetValidString("Enter the new updated text: ");

        Console.WriteLine($"ID: {habitId} -- Update option: {updateOption.ToString()} -- New entry: {updateString}");
        if (ConsoleHelper.Confirm("Confirm the update of this haibt?"))
        {
            using (var connection = new SqliteConnection($"Data Source={Filename}"))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    switch (updateOption)
                    {
                        case HabitsUpdateOptions.Name:
                            command.CommandText =
                                @"
								UPDATE Habits
								SET HabitName = $value
								WHERE HabitId = $habitId;
							";
                            break;
                        case HabitsUpdateOptions.Date:
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
    }

    /// <summary>
    /// Delete a habit from the Habits table.
    /// </summary>
    public void Delete()
    {
        Read(false);
        int habitId = ConsoleHelper.GetInt("Enter the id of the entry you'd like to change: ");

        if (ConsoleHelper.Confirm("Confirm the deletion of this habit?"))
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

    public string? GetHabitName(int habitId)
    {
        Console.WriteLine("INSIDE GetHabitName: " + habitId);
        string? habitName;
        using var connection = new SqliteConnection($"Data Source={Filename}");
        using var command = connection.CreateCommand();
        connection.Open();

        command.CommandText = "SELECT HabitName FROM Habits WHERE HabitId = $habitId";
        command.Parameters.AddWithValue("$habitId", habitId);

        using var reader = command.ExecuteReader();
        reader.Read();
        habitName = reader.GetString(0);

        return habitName;
    }
}
