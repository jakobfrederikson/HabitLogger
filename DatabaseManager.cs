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
	readonly private string Filename;

	public enum ReadOptions { All, Name, Date }
	public enum UpdateOptions { Name, Quantity, Date }

	public DatabaseManager(string Filename)
	{
		this.Filename = Filename;
		CreateHabitTableIfNotExist();
	}

	private void CreateHabitTableIfNotExist()
	{
		using (var connection = new SqliteConnection($"Data Source={this.Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();
				command.CommandText = 
					@"CREATE TABLE IF NOT EXISTS habit (
						id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
						name TEXT NOT NULL,
						quantity INTEGER,
						date TEXT NOT NULL
					);";
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
	}

	public void CreateHabit(string habitName, int habitQuantity)
	{
		using (var connection = new SqliteConnection($"Data Source={this.Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();
				command.CommandText =
					@"
						INSERT INTO habit (name, quantity, date) 
						VALUES ($name,$quantity,$date);
					";
				command.Parameters.AddWithValue("$name", habitName);
				command.Parameters.AddWithValue("$quantity", habitQuantity.ToString());
				command.Parameters.AddWithValue("$date", DateTime.UtcNow.ToShortDateString());

				try
				{
					command.ExecuteNonQuery();
				}
				catch (SqliteException e)
				{
					Console.WriteLine("Could not create a new habit! Details: " + e.Message);
				}
				finally
				{
					command.Dispose();
					connection.Dispose();
				}
			}
		}
	}

	public void ReadHabit(ReadOptions readOption, int limit = -1, long specific = -1)
	{
		using (var connection = new SqliteConnection($"Data Source={this.Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();
				var table = new ConsoleTable("id", "name", "quantity", "date");
				command.CommandText = "SELECT * FROM habit";

				switch (readOption)
				{
					case ReadOptions.All:
						command.CommandText = $"SELECT * FROM habit;";
						break;
					case ReadOptions.Name:
						command.CommandText = $"SELECT * FROM habit;";
						break;
					case ReadOptions.Date:
						command.CommandText = $"SELECT * FROM habit;";
						break;
					default:
						//command.CommandText = $"SELECT * FROM time WHERE date={day}";
						break;
				}
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var id = reader.GetString(0);
						var name = reader.GetString(1);
						var quantity = reader.GetString(2);
						var date = reader.GetString(3);
						table.AddRow(id, name, quantity, date);
					}

					table.Write();
				}
			}
		}
	}

	public void UpdateHabit(int habitId, string habitUpdateText, UpdateOptions updateOption)
	{
		using (var connection = new SqliteConnection($"Data Source={this.Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();

				switch (updateOption)
				{
					case UpdateOptions.Name:
						command.CommandText =
							@"
								UPDATE habit
								SET name = $value
								WHERE id = $habitId;
							";						
						break;
					case UpdateOptions.Quantity:
						command.CommandText =
							@"
								UPDATE habit
								SET quantity = $value
								WHERE id = $habitId;
							";
						break;
					case UpdateOptions.Date:
						command.CommandText =
							@"
								UPDATE habit
								SET date = $value
								WHERE id = $habitId;
							";
						break;
					default:
						break;
				}

				command.Parameters.AddWithValue("$value", habitUpdateText);
				command.Parameters.AddWithValue("$habitId", habitId);

				try
				{
					command.ExecuteNonQuery();
				}
				catch (SqliteException e)
				{
					Console.WriteLine("Could not insert into table! Details: " + e.Message);
				}
				finally
				{
					command.Dispose();
					connection.Dispose();
				}
			}
		}
	}

	public void DeleteHabit(int habitId)
	{
		using (var connection = new SqliteConnection($"Data Source={this.Filename}"))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();

				command.CommandText =
							@"
								DELETE FROM habit
								WHERE id = $habitId;
							";
				command.Parameters.AddWithValue("$habitId", habitId);

				try
				{
					command.ExecuteNonQuery();
				}
				catch (SqliteException e)
				{
					Console.WriteLine($"Could not delete the row {habitId}! Details: " + e.Message);
				}
				finally
				{
					command.Dispose();
					connection.Dispose();
				}
			}
		}
	}
}
