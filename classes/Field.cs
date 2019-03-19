using System;
using System.Collections.Generic;

namespace Mined_Out {

    public struct Coords {
        public Coords(int i, int j) {
            this.i = i;
            this.j = j;
        }
        public int i {set; get;}
        public int j {set; get;}
    }
    public enum Direction {
        Up,
        Right,
        Left,
        Down
    }
    public enum Event {
        Boom,
        Step,
        Nothing,
        Finished
    }
    public class Field {
        private Cell[,] field;

        public Cell this[int i, int j] {
            get {
                return field[i,j];
            }
            set {
                field[i,j] = value;
            }
        }
        private Path playerCell {get {
            return field[player.coords.i, player.coords.j] as Path;
        }}

        public int SuitableCellsAmount {get {
            int a = 0;
            for(int i = 1; i < Height - 1; i++) {
                for(int j = 1; j < Width - 1; j++) {
                    if(IsSuitable(i, j)) {
                        a++;
                    }
                }
            }
            return a;
        }}

        private Player player;
        public Coords PlayerCoords {get {
            return player.coords;
        }}
        public Field(int height, int width) {
            this.field = new Cell[height, width];
            this.initEmptyField();
        }

        public void SetPlayer(int i, int j) {
            this.player = new Player(i, j);
            this.field[i, j] = new Path(player: true);
        }
        private int CalculateMines (Coords c) {
            return this.CalculateMines(c.i, c.j);
        }

        public void SetProtected(int i, int j) {
            ((Path)field[i,j]).IsProtected = true;
        }
        public void SetProtected(Coords c) {
            SetProtected(c.i, c.j);
        }

        public bool IsSuitable(int i, int j, bool acceptProtected = false) {
            return IsSuitable(new Coords(i, j), acceptProtected);
        }
        // Used for generating levels
        public bool IsSuitable(Coords c, bool acceptProtected = false) {
            if(c.i < 0 || c.j < 0 || c.i >= Height || c.j >= Width) {
                return false;
            }
            if(field[c.i, c.j] is Wall) {
                return false;
            }
            if(isMined(c.i, c.j) || (!acceptProtected && IsProtected(c))) {
                return false;
            }
            return true;
        }
        public bool IsProtected(Coords c) {
            return ((Path)field[c.i,c.j]).IsProtected;
        }
        public void PlantMine(int i, int j) {
            ((Path)field[i,j]).IsMined = true;
        }

        public bool IsFinish(int i, int j) {
            if(!IsSuitable(i, j)) {
                return false;
            }
            if(i == 0) {
                return true;
            }
            return false;
        }
        public bool IsFinish(Coords c) {
            return IsFinish(c.i, c.j);
        }
        private int CalculateMines(int i, int j) {
            int mines = 0;

            if(i - 1 >= 0 && isMined(i - 1, j)) {
                mines++;
            }

            if(j - 1 >= 0 && isMined(i, j - 1)) {
                mines++;
            }

            if(i + 1 < field.GetLength(0) && isMined(i+1, j)) {
                mines++;
            }

            if(j + 1 < field.GetLength(1) && isMined(i, j+1)) {
                mines++;
            }
            
            return mines;
        }
        private bool isMined(int i, int j) {
            Path p = field[i, j] as Path;
            return p != null && p.IsMined;
        }
        public int Width {get {
            return field.GetLength(1);
        }}
        public int Height {get {
            return field.GetLength(0);
        }}
        private void initEmptyField() {
            for(int i = 0; i < this.field.GetLength(0); i++) {
                for(int j = 0; j < this.field.GetLength(1); j++) {
                    this.field[i, j] = new Path();
                }
            }
        }

        public bool CheckIsWinnable() {
            Queue<Coords> queue = new Queue<Coords>();
            queue.Enqueue(PlayerCoords);
            Coords c;
            while(queue.Count > 0) {
                c = queue.Dequeue();
                if(IsFinish(c)) return true;

                // Add adjacent elements
                if(IsSuitable(c.i + 1, c.j, true)) {
                    queue.Enqueue(new Coords(c.i + 1, c.j));
                }
                if(IsSuitable(c.i - 1, c.j, true)) {
                    queue.Enqueue(new Coords(c.i - 1, c.j));
                }
                if(IsSuitable(c.i, c.j + 1, true)) {
                    queue.Enqueue(new Coords(c.i, c.j + 1));
                }
                if(IsSuitable(c.i, c.j - 1, true)) {
                    queue.Enqueue(new Coords(c.i, c.j - 1));
                }
            }
            return false;
        }

        public void PrintToConsole() {
            Console.Clear();
            for(int i = 0; i < this.field.GetLength(0); i++) {
                for(int j = 0; j < this.field.GetLength(1); j++) {
                    Console.Write(this.field[i, j].Icon);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        public Event Move(Direction direction) {


            switch(direction) {
                case Direction.Down:
                    if(player.coords.i + 1 < field.GetLength(0) &&
                        !(field[player.coords.i + 1, player.coords.j] is Wall)) {
                        playerCell.PlayerLeft();
                        player.coords.i += 1;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Up:
                    if(player.coords.i - 1 >= 0 &&
                        !(field[player.coords.i - 1, player.coords.j] is Wall)) {
                        playerCell.PlayerLeft();
                        player.coords.i--;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Left:
                    if(player.coords.j - 1 >= 0 &&
                        !(field[player.coords.i, player.coords.j - 1] is Wall)) {
                        playerCell.PlayerLeft();
                        player.coords.j--;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Right:
                    if(player.coords.j + 1 < field.GetLength(1) &&
                        !(field[player.coords.i, player.coords.j + 1] is Wall)) {
                        playerCell.PlayerLeft();
                        player.coords.j++;
                    } else {
                        return Event.Nothing;
                    }
                    break;
            }
            return PlayerDidMove();
        }

        private Event PlayerDidMove() {
            if(playerCell.IsMined) {
                return Event.Boom;
            }
            char playerIcon = CalculateMines(player.coords).ToString()[0];
            playerCell.PlayerEntered(playerIcon);
            if(IsFinish(player.coords)) {
                return Event.Finished;
            }
            return Event.Step;
        }
    }
}