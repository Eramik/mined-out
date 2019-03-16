namespace Mined_Out {
    public class Path : Cell {
        public bool IsMined {private set; get;}
        public bool IsVisited {private set; get;}
        public Path(bool player = false) {
            if(player) {
                this.Icon = '0';
                this.IsMined = false;
                this.IsVisited = true;
            } else {
                this.Icon = ' ';
                this.IsMined = false;
                this.IsVisited = false;
            }
        }

        public void PlayerEntered(char newIcon = '0') {
            this.Icon = newIcon;
            this.IsVisited = true;
        }
        public void PlayerLeft() {
            this.Icon = '.';
        }
    }
}