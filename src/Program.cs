using System;
using HabitLogger.src.Database;
using HabitLogger.src.DeclarativeConsoleMenu;

namespace HabitLogger.src;

class Program
{
    static void Main(string[] args)
    {
        MenuCollection menus = MenuGenerator.CreateMenuCollection();
        menus.StartApp();
    }
}