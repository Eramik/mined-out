namespace Mined_Out {
    class Field {
        private Cell[,] field;
        Field() {
            field = new Cell[11,7];
            for(int i = 0; i < 7; i++) {
                if (i == 4) {
                    field[0, 4] = new Path(player:true);
                }
                field[0, i] = new 
            }
        }
    }
}