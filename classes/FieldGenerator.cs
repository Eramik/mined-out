using System;

namespace Mined_Out {
    public static class FieldGenerator {
        public static Field Generate(Inventory i, bool twoPlayers = false) {
            Field field;
            if(twoPlayers) {
                field = PreGenerateField(10, 10, i, twoPlayers);
            } else {
                field = PreGenerateField(10, 9, i, twoPlayers);
            }
            field = GenerateProtectedPath(field);
            int minesDensity = 28;
            if(twoPlayers) {
                minesDensity += 9;
            }
            field = EnrichWithMines(field, minesDensity);
            return field;
        }

        public static Field PreGenerateField(int height, int width, Inventory inv, bool twoPlayers = false) {
            if(twoPlayers && width % 2 == 1) {
                throw new Exception("Width of the game field has to be even");
            }
            if (!twoPlayers && width % 2 == 0) {
                throw new Exception("Width of the game field has to be odd");
            }
            Field field = new Field(height, width, inv);
            int mid = mid = (int)(width / 2);;
            if(twoPlayers) {
                mid--;
            }
            // Lower edge
            for(int i = 0; i < width; i++) {
                if (i == mid) {
                    field.SetPlayer(height - 1, mid);
                } else if (twoPlayers && i == mid + 1) {
                    field.SetPlayer(height - 1, mid + 1, 2);
                } else {
                    field[height - 1, i] = new Wall();
                }
            }

            // Left and right edge
            for(int i = 0; i < height - 1; i++) {
                field[i, 0] = new Wall();
                field[i, width - 1] = new Wall();
            }

            return field;
        }
        private static Field GenerateProtectedPath(Field field) {
            Random rnd = new Random();
            var q = Inventory.GenerateItemsToSpawn();
            Coords c = new Coords(field.PlayerCoords.i, field.PlayerCoords.j);
            int suitable = 0;
            Direction[] directions = new Direction[4];
            Direction d;
            while(!field.IsFinish(c)) {
                if(field.IsSuitable(c.i - 1, c.j)) {
                    directions[suitable++] = Direction.Up;
                }
                if(field.IsSuitable(c.i, c.j + 1)) {
                    directions[suitable++] = Direction.Right;
                }
                if(field.IsSuitable(c.i, c.j - 1)) {
                    directions[suitable++] = Direction.Left;
                }

                // Choose direction
                d = directions[rnd.Next(suitable)];
                if(d == Direction.Up) {
                    c.i--;
                } else if(d == Direction.Left) {
                    c.j--;
                } else if(d == Direction.Right) {
                    c.j++;
                }
                if(!field.IsFinish(c)) {
                    field.SetProtected(c);
                    if(q.Count != 0 && rnd.Next(100) < 8) {
                        ((Path)field[c.i, c.j]).PutItem(q.Dequeue());
                    }
                }
                else {
                    break;
                }
                suitable = 0;
            }
            return field;
        }
        
        // Density in percents (0 - 100)
        private static Field EnrichWithMines(Field field, int density = 28) {
            Random rnd = new Random();
            int minesToPlant = (int)((double)field.SuitableCellsAmount / 100 * density);
            int i, j;
            while(minesToPlant > 0) {
                i = rnd.Next(1, field.Height - 1);
                j = rnd.Next(1, field.Width - 1);
                if(field.IsSuitable(i, j)) {
                    field.PlantMine(i, j);
                    minesToPlant--;
                }
                field.PrintToConsole();
                System.Console.WriteLine(minesToPlant);
            }
            return field;
        }
    }
}