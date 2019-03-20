using System;

namespace Mined_Out {
    public class Scanner : InventoryItem {
        new public static int ChanceToSpawn = 25;
        public Scanner() {
            this.Icon = '⎚';
            this.Type = "Scanner";
        }

        override public bool Activate(Field f) {
            Coords? c = Move(f, f.PlayerCoords);

            if(c == null) return false;
            Coords co = (Coords)c;

            Expose(f, co);
            return true;
        }
        private void Expose(Field f, Coords c) {

            if(f.IsPath(c.i, c.j)) {
                ((Path)f[c.i, c.j]).Expose();
            }

            if(f.IsPath(c.i + 1, c.j)) {
                ((Path)f[c.i + 1, c.j]).Expose();
            }

            if(f.IsPath(c.i - 1, c.j)) {
                ((Path)f[c.i - 1, c.j]).Expose();
            }

            if(f.IsPath(c.i, c.j + 1)) {
                ((Path)f[c.i, c.j + 1]).Expose();
            }

            if(f.IsPath(c.i, c.j - 1)) {
                ((Path)f[c.i, c.j - 1]).Expose();
            }

            if(f.IsPath(c.i + 1, c.j + 1)) {
                ((Path)f[c.i + 1, c.j + 1]).Expose();
            }

            if(f.IsPath(c.i - 1, c.j - 1)) {
                ((Path)f[c.i - 1, c.j - 1]).Expose();
            }

            if(f.IsPath(c.i - 1, c.j + 1)) {
                ((Path)f[c.i - 1, c.j + 1]).Expose();
            }

            if(f.IsPath(c.i + 1, c.j - 1)) {
                ((Path)f[c.i + 1, c.j - 1]).Expose();
            }
        }
        private void Draw(Field f, Coords c) {
            if(f.IsPath(c.i, c.j)) {
                ((Path)f[c.i, c.j]).Select();
            }

            if(f.IsPath(c.i + 1, c.j)) {
                ((Path)f[c.i + 1, c.j]).Select();
            }

            if(f.IsPath(c.i - 1, c.j)) {
                ((Path)f[c.i - 1, c.j]).Select();
            }

            if(f.IsPath(c.i, c.j + 1)) {
                ((Path)f[c.i, c.j + 1]).Select();
            }

            if(f.IsPath(c.i, c.j - 1)) {
                ((Path)f[c.i, c.j - 1]).Select();
            }

            if(f.IsPath(c.i + 1, c.j + 1)) {
                ((Path)f[c.i + 1, c.j + 1]).Select();
            }

            if(f.IsPath(c.i - 1, c.j - 1)) {
                ((Path)f[c.i - 1, c.j - 1]).Select();
            }

            if(f.IsPath(c.i - 1, c.j + 1)) {
                ((Path)f[c.i - 1, c.j + 1]).Select();
            }

            if(f.IsPath(c.i + 1, c.j - 1)) {
                ((Path)f[c.i + 1, c.j - 1]).Select();
            }

            f.PrintToConsole();

            if(f.IsPath(c.i, c.j)) {
                ((Path)f[c.i, c.j]).Unselect();
            }

            if(f.IsPath(c.i + 1, c.j)) {
                ((Path)f[c.i + 1, c.j]).Unselect();
            }

            if(f.IsPath(c.i - 1, c.j)) {
                ((Path)f[c.i - 1, c.j]).Unselect();
            }

            if(f.IsPath(c.i, c.j + 1)) {
                ((Path)f[c.i, c.j + 1]).Unselect();
            }

            if(f.IsPath(c.i, c.j - 1)) {
                ((Path)f[c.i, c.j - 1]).Unselect();
            }

            if(f.IsPath(c.i + 1, c.j + 1)) {
                ((Path)f[c.i + 1, c.j + 1]).Unselect();
            }

            if(f.IsPath(c.i - 1, c.j - 1)) {
                ((Path)f[c.i - 1, c.j - 1]).Unselect();
            }

            if(f.IsPath(c.i - 1, c.j + 1)) {
                ((Path)f[c.i - 1, c.j + 1]).Unselect();
            }

            if(f.IsPath(c.i + 1, c.j - 1)) {
                ((Path)f[c.i + 1, c.j - 1]).Unselect();
            }
        }
        private Coords? Move(Field f, Coords c) {
            while(true) {
                Draw(f, c);

                var k = Console.ReadKey(true);
                var key = k.Key;
                
                if(key == ConsoleKey.W || key == ConsoleKey.UpArrow) {
                    if(!f.IsPath(c.i -1, c.j)) {
                        continue;
                    }
                    c.i--;
                } else if(key == ConsoleKey.D || key == ConsoleKey.RightArrow) {
                    if(!f.IsPath(c.i, c.j +1)) {
                        continue;
                    }
                    c.j++;
                } else if(key == ConsoleKey.S || key == ConsoleKey.DownArrow) {
                    if(!f.IsPath(c.i +1, c.j)) {
                        continue;
                    }
                    c.i++;
                } else if(key == ConsoleKey.A || key == ConsoleKey.LeftArrow) {
                    if(!f.IsPath(c.i, c.j -1)) {
                        continue;
                    }
                    c.j--;
                } else if(key == ConsoleKey.Enter || key == ConsoleKey.Spacebar) {
                    return c;
                } else if(key == ConsoleKey.Escape) {
                    return null;
                }
            }
        }
    }
}