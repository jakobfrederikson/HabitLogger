using HabitLogger.src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.src.DeclarativeConsoleMenu;

/// <summary>
/// All menus for the habit logger are created here.
/// </summary>
public class MenuGenerator
{
    public static MenuCollection CreateMenuCollection()
    {
        MenuCollection collection = new MenuCollection();

        return new MenuCollection()
        {
            Menus =
            {
                new Menu()
                {
                    Title = "Main Menu",
                    MenuId = 1,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Create a habit",
                            SubMenuId = 11
                        },
                        new MenuItem()
                        {
                            Text = "View habits",
                            SubMenuId = 12
                        },
                        new MenuItem()
                        {
                            Text = "Edit a habit",
                            SubMenuId = 13
                        },
                        new MenuItem()
                        {
                            Text = "Delete a habit",
                            SubMenuId = 14
                        },
                        new MenuItem()
                        {
                            Text = "View logging options",
                            SubMenuId = 2
                        },
                        new MenuItem()
                        {
                            Text = "Exit",
                            Action = () => { Environment.Exit(0); }
                        }
                    }
                },
                new Menu()
                {
                    Title = "Create a habit",
                    MenuId = 11,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Continue to create a habit",
                            Action = () => DatabaseManager.Instance.Habits.Create()
                        },
                        new MenuItem()
                        {
                            Text = "Back to main menu",
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "View habits",
                    MenuId = 12,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "View all habits",
                            Action = () => DatabaseManager.Instance.Habits.Read(true)
                        },
                        new MenuItem()
                        {
                            Text = "Back to main menu",
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Edit a habit",
                    MenuId = 13,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Continue to edit a habit",
                            Action = () => DatabaseManager.Instance.Habits.Update()
                        },
                        new MenuItem()
                        {
                            Text = "Back to main menu",
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Delete a habit",
                    MenuId = 14,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Continue to delete a habit",
                            Action = () => DatabaseManager.Instance.Habits.Delete()
                        },
                        new MenuItem()
                        {
                            Text = "Back to main menu",
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Logging Options",
                    MenuId = 2,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Create a log",
                            SubMenuId = 21
                        },
                        new MenuItem()
                        {
                            Text = "View a habits log",
                            SubMenuId = 22
                        },
                        new MenuItem()
                        {
                            Text = "Edit a log",
                            SubMenuId = 23
                        },
                        new MenuItem()
                        {
                            Text = "Delete a log",
                            SubMenuId = 24
                        },
                        new MenuItem()
                        {
                            Text = "Back to main menu",
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Create a log",
                    MenuId = 21,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Continue to creating a log",
                            Action = () => DatabaseManager.Instance.HabitsTracker.Create()
                        },
                        new MenuItem()
                        {
                            Text = "Back to logging options",
                            SubMenuId = 2
                        }
                    }
                },
                new Menu()
                {
                    Title = "View a log",
                    MenuId = 22,
                    MenuItems = 
                    {
                        new MenuItem()
                        {
                            Text = "View log details",
                            Action = () => DatabaseManager.Instance.HabitsTracker.Read(true)
                        },
                        new MenuItem()
                        {
                            Text = "Back to logging options",
                            SubMenuId = 2
                        }
                    }
                },
                new Menu()
                {
                    Title = "Edit a log",
                    MenuId = 23,
                    MenuItems = 
                    {
                        new MenuItem()
                        {
                            Text = "Edit log details",
                            Action = () => DatabaseManager.Instance.HabitsTracker.Update()
                        },
                        new MenuItem()
                        {
                            Text = "Back to logging options",
                            SubMenuId = 2
                        }
                    }
                },
                new Menu()
                {
                    Title = "Delete a log",
                    MenuId = 24,
                    MenuItems = 
                    {
                    new MenuItem()
                        {
                            Text = "Delete log",
                            Action = () => DatabaseManager.Instance.HabitsTracker.Delete()
                        },
                        new MenuItem()
                        {
                            Text = "Back to logging options",
                            SubMenuId = 2
                        }
                    }
                },
            }
        };
    }
}
