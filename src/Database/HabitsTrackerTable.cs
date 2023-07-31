using ConsoleTables;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;

public class HabitsTrackerTable : IHabitLoggerTable
{
    public string? Filename { get; set; }

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
	/// <param name="habitId">The id of the parent to which you are tracking.</param>
	/// <param name="habitQuantity">The quantity of times you completed this habit.</param>
	public void Create(string habitName = "default", int habitId = 0, int habitQuantity = 0)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
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
    }

    /// <summary>
    /// View all entries in the HabitsTracker table.
    /// </summary>
    public void Read()
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                var table = new ConsoleTable("Id", "Quantity", "Date Logged", "Foreign Key");
                command.CommandText = "SELECT * FROM HabitsTracker";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetString(0);
                        var quantity = reader.GetString(1);
                        var dateLogged = reader.GetString(2);
                        var foreignKey = reader.GetString(3);
                        table.AddRow(id, quantity, dateLogged, foreignKey);
                    }

                    table.Write();
                }
            }
        }
    }

    /// <summary>
    /// Update the quantity, date, or foreign key of an entry in the HabitsTracker table.
    /// </summary>
    /// <param name="entryId">The ID of the entry.</param>
    /// <param name="updateString">The string to replace the old data.</param>
    /// <param name="updateOption">Specify the data to update.</param>
    public void Update(int entryId, string updateString, UpdateOptions updateOption)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                switch (updateOption)
                {
                    case UpdateOptions.Date:
                        command.CommandText =
                            @"
								UPDATE HabitsTracker
								SET HabitName = $value
								WHERE HabitId = $entryId;
							";
                        break;
                    case UpdateOptions.Tquantity:
                        command.CommandText =
                            @"
								UPDATE HabitsTracker
								SET EntryQuantity = $value
								WHERE EntryId = $entryId;
							";
                        break;
                    case UpdateOptions.TforeignKey:
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
    }

    /// <summary>
    /// Delete an from the HabitsTracker table.
    /// </summary>
    /// <param name="entryId">The id of the entry to delete.</param>
    public void Delete(int entryId)
    {
        using (var connection = new SqliteConnection($"Data Source={Filename}"))
        {
            using (var command = connection.CreateCommand())
            {
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
}
