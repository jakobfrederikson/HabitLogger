using ConsoleTables;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;

public enum LoggingUpdateOptions { Date, Quantity, ForeginKey }
public class HabitsTrackerTable
{
    public HabitsTrackerTable()
    {
        Filename = "habitlogger.db";
        CreateTableIfNotExists();
    }

    public string? Filename { get; }
	public bool Empty => isEmpty();

	public void CreateTableIfNotExists()
    {
        using var connection = new SqliteConnection($"Data Source={Filename}");
        using var command = connection.CreateCommand();            
        connection.Open();

        command.CommandText =
            @" 
				CREATE TABLE IF NOT EXISTS HabitsTracker(
					EntryId         INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
					EntryQuantity   INTEGER NOT NULL,
					Date            TEXT NOT NULL,
					HabitId         INTEGER NOT NULL,
					FOREIGN KEY(HabitId) REFERENCES Habits(HabitId)
				);
			";

        TableHelper.TryExecuteNonQuery(connection, command);
    }

	/// <summary>
	/// Returns true if the table is empty.
	/// </summary>
	private bool isEmpty()
	{
		using (var connection = new SqliteConnection($"Data Source={Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();
				command.CommandText =
					@" 
						SELECT * FROM HabitsTracker;
					";

				using var reader = command.ExecuteReader();
                return reader.HasRows ? false: true;
			}
		}
	}

	/// <summary>
	/// Create an entry to be inserted in the HabitsTracker table.
	/// </summary>
	public void Create()
    {
        // Show habits table first
        DatabaseManager.Instance.Habits.Read(false);

        int habitId = ConsoleHelper.GetInt("Enter the ID of the habit you're logging: ");
        int habitQuantity = ConsoleHelper.GetInt("Enter the amount of times this habit was performed today: ");

        if (ConsoleHelper.Confirm("Confirm the the creation of this log?"))
        {
            using var connection = new SqliteConnection($"Data Source={Filename}");
            using var command = connection.CreateCommand();                
            connection.Open();

            command.CommandText =
                @"
				INSERT INTO HabitsTracker (EntryQuantity, Date, HabitId) 
				VALUES ($q,$d,$id);
			";
            command.Parameters.AddWithValue("$q", habitQuantity);
            command.Parameters.AddWithValue("$d", DateTime.Now.ToShortDateString());
            command.Parameters.AddWithValue("$id", habitId);

            TableHelper.TryExecuteNonQuery(connection, command);   
        }        
    }

    /// <summary>
    /// View all entries in the HabitsTracker table.
    /// </summary>
    public void Read(bool waitForUserResponse)
    {
        using var connection = new SqliteConnection($"Data Source={Filename}");
        using var command = connection.CreateCommand();
        connection.Open();

        var table = new ConsoleTable("Id", "Quantity", "Date Logged", "Parent", "Foreign Key");
        command.CommandText = "SELECT * FROM HabitsTracker";

        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetString(0);
            var quantity = reader.GetString(1);
            var dateLogged = reader.GetString(2);
            var foreignKey = reader.GetString(3);
            var parent = DatabaseManager.Instance.Habits.GetHabitName(Convert.ToInt32(foreignKey));
            table.AddRow(id, quantity, dateLogged, parent, foreignKey);
        }
        table.Write();
                
        if (waitForUserResponse)
        {
            Console.Write("Press any key to continue. . .");
            Console.ReadKey(false);
        }
    }

    /// <summary>
    /// Update the quantity, date, or foreign key of an entry in the HabitsTracker table.
    /// </summary>
    public void Update()
    {
        Read(false);
        int entryId = ConsoleHelper.GetInt("Enter the id of the entry you'd like to change: ");
        Console.WriteLine("[0] Date (d-mm-yyyy)");
        Console.WriteLine("[1] Quantity");
        Console.WriteLine("[2] Foreign Key");
        LoggingUpdateOptions updateOption = (LoggingUpdateOptions)ConsoleHelper.GetInt("Choose what you would like to update: ", 2);
        string updateString = ConsoleHelper.GetValidString("Enter the new updated text: ");

        Console.WriteLine($"ID: {entryId} -- Update option: {updateOption.ToString()} -- New entry: {updateString}");
        if (ConsoleHelper.Confirm("Confirm this update?"))
        {
            using var connection = new SqliteConnection($"Data Source={Filename}");
            using var command = connection.CreateCommand();                
            connection.Open();

            switch (updateOption)
            {
                case LoggingUpdateOptions.Date:
                    command.CommandText =
                        @"
						UPDATE HabitsTracker
						SET HabitName = $value
						WHERE HabitId = $entryId;
					";
                    break;
                case LoggingUpdateOptions.Quantity:
                    command.CommandText =
                        @"
						UPDATE HabitsTracker
						SET EntryQuantity = $value
						WHERE EntryId = $entryId;
					";
                    break;
                case LoggingUpdateOptions.ForeginKey:
                    command.CommandText =
                        @"
						UPDATE HabitsTracker
						SET HabitId = $value
						WHERE EntryId = $entryId;
					";
                    break;
                default:
                    break;
            }

            command.Parameters.AddWithValue("$value", updateString);
            command.Parameters.AddWithValue("$entryId", entryId);

            TableHelper.TryExecuteNonQuery(connection, command);
        }        
    }

    /// <summary>
    /// Delete an from the HabitsTracker table.
    /// </summary>
    public void Delete()
    {
        Read(false);
        int entryId = ConsoleHelper.GetInt("Enter the id of the entry you'd like to change: ");

        if (ConsoleHelper.Confirm("Confirm the deletion of this log?"))
        {
            using var connection = new SqliteConnection($"Data Source={Filename}");
            using var command = connection.CreateCommand();                
            connection.Open();

            command.CommandText =
                        @"
						DELETE FROM HabitsTracker
						WHERE EntryId = $entryId;
					";
            command.Parameters.AddWithValue("$entryId", entryId);

            TableHelper.TryExecuteNonQuery(connection, command);
        }        
    }
}
