using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.Database;

public enum UpdateOptions { None, Date, Hname, Tquantity, TforeignKey }
public interface IHabitLoggerTable
{
    public string? Filename { get; set; }
    void CreateTableIfNotExists();
    void Create(string habitName = "default", int habitId = 0, int habitQuantity = 0);
    void Read();
    void Update(int id, string updateString, UpdateOptions updateOption);
    void Delete(int id);
}
