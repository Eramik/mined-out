namespace Mined_Out {
    public class Path : Cell {
        public bool IsMined {set {
            isMined = value;
            //if(isMined)
            //Icon = 'M';
        } get {return isMined;}}
        private bool isMined;
        public bool IsVisited {private set; get;}
        private bool isProtected;
        public bool IsProtected {set {
            this.isProtected = value;
            //if(isProtected)
            //this.Icon = 'P';
        } get {return this.isProtected;}}
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