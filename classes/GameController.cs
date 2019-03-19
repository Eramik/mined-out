using System;

namespace Mined_Out {
    public class GameController {
        private Field field;
        private void Generate() {
            this.field = FieldGenerator.Generate();
        }
        public void Run() {
            RunMainMenu();
        }
        private void RunGame() {
            while(true) {
                this.Generate();
                field.PrintToConsole();
                this.Walking();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }

        private void Walking() {
            while(true) {
                var consoleKey = Console.ReadKey(true);
                ConsoleKey key = consoleKey.Key;
                Direction d;
                if(key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.S) {
                    d = Direction.Down;
                } else if(key == ConsoleKey.UpArrow || 
                    key == ConsoleKey.W) {
                    d = Direction.Up;
                } else if(key == ConsoleKey.LeftArrow || 
                    key == ConsoleKey.A) {
                    d = Direction.Left;
                } else if(key == ConsoleKey.RightArrow || 
                    key == ConsoleKey.D) {
                    d = Direction.Right;
                } else if(key == ConsoleKey.Escape) {
                    AskExitToMenu();
                    field.PrintToConsole();
                    continue;
                } else if(key == ConsoleKey.C) { 
                    string message;
                    if(field.CheckIsWinnable()) {
                        message = "The level is winnable!";
                    } else {
                        message = "The leverl is NOT winnable :(";
                    }
                    NotifyUser(message);
                    field.PrintToConsole();
                    continue;
                } else {
                    continue;
                }
                
                Event result = field.Move(d);
                if (result == Event.Boom) {
                    Console.Clear();
                    Console.WriteLine("YOU DIED!");
                    return;
                }
                if (result == Event.Finished) {
                    Console.Clear();
                    Console.WriteLine("$$$ YOU WON $$$");
                    return;
                }
                if(result != Event.Nothing) {
                    field.PrintToConsole();
                }
                
            }
        }

        private void NotifyUser(string message) {
            string[] menuOptions = new string[1];
            menuOptions[0] = "OK";
            MenuAction[] menuActions = new MenuAction[1];
            menuActions[0] = null;
            RunMenu(menuOptions, menuActions, message);
        }
        private void AskExitToMenu() {
            string[] menuOptions = new string[2];
            menuOptions[0] = "NO";
            menuOptions[1] = "YES";
            MenuAction[] actions = new MenuAction[2];
            actions[0] = null;
            actions[1] = RunMainMenu;
            string header = "Do you want to exit to game menu?";
            RunMenu(menuOptions, actions, header);
        }
        private delegate void MenuAction();
        private void RunMainMenu() {
            string[] menuOptions = new string[2];
            menuOptions[0] = "Start game";
            //menuOptions[1] = "Two Players";
            //menuOptions[2] = "Level editor";
            //menuOptions[3] = "Help";
            menuOptions[1] = "Exit";
            MenuAction[] actions = new MenuAction[2];
            actions[0] = StartGame;
            actions[1] = Exit;
            RunMenu(menuOptions, actions, "GAME MENU");  
        }
        private void RunMenu(string[] menuOptions, MenuAction[] actions, string header = "", int active = 0) {
            while(true) {
                PrintMenu(menuOptions, header, active);
                var key = Console.ReadKey(true);
                var Key = key.Key;
                if(Key == ConsoleKey.Enter || Key == ConsoleKey.Spacebar) {
                    if(actions[active] == null) return;
                    actions[active].Invoke();
                } else if(Key == ConsoleKey.UpArrow || Key == ConsoleKey.W) {
                    if(active > 0) {
                        active--;
                    }
                } else if(Key == ConsoleKey.DownArrow || Key == ConsoleKey.S) {
                    if(active < menuOptions.Length - 1) {
                        active++;
                    }
                }
            }
        }
        private void StartGame() {
            RunGame();
        }
        private void Exit() {
            Console.Clear();
            Console.WriteLine("Have a nice day!");
            System.Environment.Exit(0);
        }
        private void PrintMenu(string[] menuOptions, string header, int active) {
            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine();
            Console.WriteLine();
            for(int i = 0; i < menuOptions.Length; i++) {
                if(i == active) {
                    Console.WriteLine(menuOptions[i] + " <<<");
                } else {
                    Console.WriteLine(menuOptions[i]);
                }
                Console.WriteLine();
            }
        }

    }
}