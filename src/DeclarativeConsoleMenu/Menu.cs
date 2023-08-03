/*
 *  Nuget package for DeclarativeConsoleMenu didn't work, so I've just copied the files from github.
 *  https://github.com/alsiola/DeclarativeConsoleMenu/tree/master 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.DeclarativeConsoleMenu;

public class Menu
{
    public Menu()
    {
        MenuItems = new List<MenuItem>();
    }

    public virtual int MenuId { get; set; }

    public virtual List<MenuItem> MenuItems { get; set; }

    public virtual string? Title { get; set; }

    public virtual void PrintToConsole()
    {
        if (!string.IsNullOrEmpty(Title))
        {
            Console.WriteLine(Title);
        }
        
        foreach (MenuItem item in MenuItems)
        {
            Console.WriteLine($"[{MenuItems.IndexOf(item)}] " + item.Text);
        }
    }

    public static Menu Create(int id, string title)
    {
        return new Menu()
        {
            Title = title,
            MenuId = id
        };
    }

    public static Menu Create(int id, string title, IEnumerable<MenuItem> menuItems)
    {
        Menu menu = new Menu()
        {
            Title = title,
            MenuId = id
        };

        menu.MenuItems.AddRange(menuItems);

        return menu;
    }

    public static Menu Create(int id, string title, params MenuItem[] menuItems)
    {
        return Create(id, title, menuItems);
    }
}