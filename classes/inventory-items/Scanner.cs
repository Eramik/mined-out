using System;

namespace Mined_Out {
    public class Scanner : InventoryItem {
        new public static int ChanceToSpawn = 25;
        new public const string Type = "Scanner";
        public Scanner() {
            this.Icon = '⎚';
        }

        override public bool Activate(Field f) {
            Coords? c = Move(f, f.PlayerCoords);

            if(c == null) return false;
            Coords co = (Coords)c;

            Expose(f, co);
            return true;
        }
        private void Expose(Field f, Coords c) {

            if(f.IsSuitable(c.i, c.j, true)) {
                ((Path)f[c.i, c.j]).Expose();
            }

            if(f.IsSuitable(c.i + 1, c.j, true)) {
                ((Path)f[c.i + 1, c.j]).Expose();
            }

            if(f.IsSuitable(c.i - 1, c.j, true)) {
                ((Path)f[c.i - 1, c.j]).Expose();
            }

            if(f.IsSuitable(c.i, c.j + 1, true)) {
                ((Path)f[c.i, c.j + 1]).Expose();
            }

            if(f.IsSuitable(c.i, c.j - 1, true)) {
                ((Path)f[c.i, c.j - 1]).Expose();
            }

            if(f.IsSuitable(c.i + 1, c.j + 1, true)) {
                ((Path)f[c.i + 1, c.j + 1]).Expose();
            }

            if(f.IsSuitable(c.i - 1, c.j - 1, true)) {
                ((Path)f[c.i - 1, c.j - 1]).Expose();
            }

            if(f.IsSuitable(c.i - 1, c.j + 1, true)) {
                ((Path)f[c.i - 1, c.j + 1]).Expose();
            }

            if(f.IsSuitable(c.i + 1, c.j - 1, true)) {
                ((Path)f[c.i + 1, c.j - 1]).Expose();
            }
        }
        private void Draw(Field f, Coords c) {
            if(f.IsSuitable(c.i, c.j, true)) {
                ((Path)f[c.i, c.j]).Select();
            }

            if(f.IsSuitable(c.i + 1, c.j, true)) {
                ((Path)f[c.i + 1, c.j]).Select();
            }

            if(f.IsSuitable(c.i - 1, c.j, true)) {
                ((Path)f[c.i - 1, c.j]).Select();
            }

            if(f.IsSuitable(c.i, c.j + 1, true)) {
                ((Path)f[c.i, c.j + 1]).Select();
            }

            if(f.IsSuitable(c.i, c.j - 1, true)) {
                ((Path)f[c.i, c.j - 1]).Select();
            }

            if(f.IsSuitable(c.i + 1, c.j + 1, true)) {
                ((Path)f[c.i + 1, c.j + 1]).Select();
            }

            if(f.IsSuitable(c.i - 1, c.j - 1, true)) {
                ((Path)f[c.i - 1, c.j - 1]).Select();
            }

            if(f.IsSuitable(c.i - 1, c.j + 1, true)) {
                ((Path)f[c.i - 1, c.j + 1]).Select();
            }

            if(f.IsSuitable(c.i + 1, c.j - 1, true)) {
                ((Path)f[c.i + 1, c.j - 1]).Select();
            }

            f.PrintToConsole();

            if(f.IsSuitable(c.i, c.j, true)) {
                ((Path)f[c.i, c.j]).Unselect();
            }

            if(f.IsSuitable(c.i + 1, c.j, true)) {
                ((Path)f[c.i + 1, c.j]).Unselect();
            }

            if(f.IsSuitable(c.i - 1, c.j, true)) {
                ((Path)f[c.i - 1, c.j]).Unselect();
            }

            if(f.IsSuitable(c.i, c.j + 1, true)) {
                ((Path)f[c.i, c.j + 1]).Unselect();
            }

            if(f.IsSuitable(c.i, c.j - 1, true)) {
                ((Path)f[c.i, c.j - 1]).Unselect();
            }

            if(f.IsSuitable(c.i + 1, c.j + 1, true)) {
                ((Path)f[c.i + 1, c.j + 1]).Unselect();
            }

            if(f.IsSuitable(c.i - 1, c.j - 1, true)) {
                ((Path)f[c.i - 1, c.j - 1]).Unselect();
            }

            if(f.IsSuitable(c.i - 1, c.j + 1, true)) {
                ((Path)f[c.i - 1, c.j + 1]).Unselect();
            }

            if(f.IsSuitable(c.i + 1, c.j - 1, true)) {
                ((Path)f[c.i + 1, c.j - 1]).Unselect();
            }
        }
        private Coords? Move(Field f, Coords c) {
            while(true) {
                Draw(f, c);

                var k = Console.ReadKey(true);
                var key = k.Key;
                
                if(key == ConsoleKey.W || key == ConsoleKey.UpArrow) {
                    if(!f.IsSuitable(c.i -1, c.j, true)) {
                        continue;
                    }
                    c.i--;
                } else if(key == ConsoleKey.D || key == ConsoleKey.RightArrow) {
                    if(!f.IsSuitable(c.i, c.j +1, true)) {
                        continue;
                    }
                    c.j++;
                } else if(key == ConsoleKey.S || key == ConsoleKey.DownArrow) {
                    if(!f.IsSuitable(c.i +1, c.j, true)) {
                        continue;
                    }
                    c.i++;
                } else if(key == ConsoleKey.A || key == ConsoleKey.LeftArrow) {
                    if(!f.IsSuitable(c.i, c.j -1, true)) {
                        continue;
                    }
                    c.i--;
                } else if(key == ConsoleKey.Enter || key == ConsoleKey.Spacebar) {
                    return c;
                } else if(key == ConsoleKey.Escape) {
                    return null;
                }
            }
        }
    }
}