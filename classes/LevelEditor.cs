using System;

namespace Mined_Out {
    public class LevelEditor {
        private Field field;
        Action[] actions = new Action[6];
        delegate bool Action(Coords c);
        public LevelEditor() {
            actions[0] = ReplacePlayer;
            actions[1] = PlaceMine;
            actions[2] = PlaceWall;
            actions[3] = PlaceScanner;
            actions[4] = Clear;
            actions[5] = FinishEditing;
        }

        private bool ReplacePlayer(Coords c) {
            field.ReplacePlayer(c);
            return true;
        }

        private bool Clear(Coords c) {
            field.ClearCell(c);
            ((Path)field[c.i, c.j]).Expose();
            return true;
        }

        private bool FinishEditing(Coords c) {
            if(!field.HasPlayer1) {
                GameController.NotifyUser("Level must have a player plced");
                return true;
            }
            bool isWinnable = field.CheckIsWinnable();
            if(!isWinnable) {
                GameController.NotifyUser("Level must be winnable");
                return true;
            }
            return false;
        }

        private bool PlaceMine(Coords c) {
            field.PlantMine(c.i, c.j);
            ((Path)field[c.i, c.j]).Expose();
            return true;
        }

        private bool PlaceWall(Coords c) {
            field.PlaceWall(c);
            return true;
        }

        private bool PlaceScanner(Coords c) {
            field.PlaceScanner(c);
            return true;
        }

        public Field Launch(Inventory inventory) {
            int height;
            while(true) {
                string heightStr = GameController.Prompt("Enter height of the game field:");
                try {
                    height = Int32.Parse(heightStr);
                } catch (Exception e) {
                    GameController.NotifyUser("Please, enter a number");
                    continue;
                }
                if(height < 7) {
                    GameController.NotifyUser("Height has to be 7 or more");
                    continue;
                }
                if(height > 20) {
                    GameController.NotifyUser("Height has to be 20 or less");
                    continue;
                } 
                break;
            }
            int width;
            while(true) {
                string widthStr = GameController.Prompt("Enter width of the game field:");
                try {
                    width = Int32.Parse(widthStr);
                } catch (Exception e) {
                    GameController.NotifyUser("Please, enter a number");
                    continue;
                }
                if(width < 5) {
                    GameController.NotifyUser("Width has to be 5 or more");
                    continue;
                }
                if(width > 25) {
                    GameController.NotifyUser("Width has to be 25 or less");
                    continue;
                } 
                if(width % 2 == 0) {
                    GameController.NotifyUser("Width has to be odd");
                    continue;
                }
                break;
            }
            this.field = FieldGenerator.PreGenerateField(height, width, inventory, false);

            return RunLevelEditor();
        }

        private string[] GetEditorMenu(int active = 2) {
            string[] editorMenu = new string[8];
            editorMenu[0] = "Editor:";
            editorMenu[1] = "";
            editorMenu[2] = "Replace player";
            editorMenu[3] = "Place mine";
            editorMenu[4] = "Place wall";
            editorMenu[5] = "Place scanner";
            editorMenu[6] = "Clear the cell";
            editorMenu[7] = "Finish editing";
            editorMenu[active] += " <<<";
            return editorMenu;
        }

        private Field RunLevelEditor() {
            Coords c = field.PlayerCoords;
            field.Expose();
            var f = field;
            int active = 2;
            while(true) {
                Print(c, active);

                var k = Console.ReadKey(true);
                var key = k.Key;
                
                if(key == ConsoleKey.W || key == ConsoleKey.UpArrow) {
                    if(!f.IsInField(c.i -1, c.j)) {
                        continue;
                    }
                    c.i--;
                } else if(key == ConsoleKey.D || key == ConsoleKey.RightArrow) {
                    if(!f.IsInField(c.i, c.j +1)) {
                        continue;
                    }
                    c.j++;
                } else if(key == ConsoleKey.S || key == ConsoleKey.DownArrow) {
                    if(!f.IsInField(c.i +1, c.j)) {
                        continue;
                    }
                    c.i++;
                } else if(key == ConsoleKey.A || key == ConsoleKey.LeftArrow) {
                    if(!f.IsInField(c.i, c.j -1)) {
                        continue;
                    }
                    c.j--;
                } else if(key == ConsoleKey.Tab) {
                    if(active == 7) {
                        active = 2;
                    } else {
                        active++;
                    }
                } else if(key == ConsoleKey.Enter || key == ConsoleKey.Spacebar) {
                    bool res = actions[active - 2].Invoke(c);
                    if(!res) {
                        field.Hide();
                        return field;
                    }
                } else if(key == ConsoleKey.C) {
                    string message;
                    if(field.CheckIsWinnable()) {
                        message = "The level is winnable!";
                    } else {
                        message = "The level is not winnable";
                    }
                    GameController.NotifyUser(message);
                    Print(c, active);
                    continue;
                } else if(key == ConsoleKey.Escape) {
                    AskExitToMenu();
                    Print(c, active);
                    continue;
                }
            }
        }

        private void AskExitToMenu() {
            string[] menuOptions = new string[2];
            menuOptions[0] = "NO";
            menuOptions[1] = "YES";
            MenuAction[] actions = new MenuAction[2];
            actions[0] = null;
            actions[1] = RunMainMenu;
            string header = "Do you want to exit to game menu?";
            GameController.RunMenu(menuOptions, actions, header);
        }

        private void RunMainMenu() {
            GameController g = new GameController();
            g.Run();
        }

        private void Print(Coords current, int active = 2) {
            field[current.i, current.j].Select();
            field.PrintToConsole(GetEditorMenu(active));
            field[current.i, current.j].Unselect();
        }
    }
}