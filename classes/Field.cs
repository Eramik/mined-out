using System;

namespace Mined_Out {

    public struct Coords {
        public Coords(int i, int j) {
            this.i = i;
            this.j = j;
        }
        public int i {private set; get;}
        public int j {private set; get;}
    }

    public class Field {
        private Cell[,] field;
        private Coords playerCoords;
        public Field() {
            this.field = new Cell[7,7];
            this.initField();

            // Lower edge
            for(int i = 0; i < 7; i++) {
                if (i == 3) {
                    this.field[6, 3] = new Path(player:true);
                    this.playerCoords = new Coords(6, 3);
                } else {
                    this.field[6, i] = new Wall();
                }
            }

            // Left and right edge
            for(int i = 0; i < 6; i++) {
                this.field[i, 0] = new Wall();
                this.field[i, 6] = new Wall();
            }
        }

        private void initField() {
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
    }
}