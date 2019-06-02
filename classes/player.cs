namespace Mined_Out {
    public class Player {
        public Coords coords;

        public Player(int i, int j) {
            coords = new Coords(i, j);
        }

        public Player (Coords coords) {
            this.coords = coords;
        }
    }
}