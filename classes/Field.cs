using System;

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
        Nothing
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
            return (Path)field[player.coords.i, player.coords.j];
        }}

        private Player player;
        public Field(int height, int width) {
            this.field = new Cell[height, width];
            this.initEmptyField();
        }

        public void SetPlayer(int i, int j) {
            this.player = new Player(i, j);
            this.field[i, j] = new Path(player: true);
        }

        private void initEmptyField() {
            for(int i = 0; i < this.field.GetLength(0); i++) {
                for(int j = 0; j < this.field.GetLength(1); j++) {
                    this.field[i, j] = new Path();
                }
            }
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
            playerCell.PlayerEntered();
            return Event.Step;
        }
    }
}