namespace Mined_Out {
    public class Path : Cell {
        public bool IsMined {set {
            isMined = value;
            //if(isMined)
            //Icon = 'M';
        } get {return isMined;}}
        private bool isMined;
        private InventoryItem item = null;
        public bool ContainsItem {get {
            return item != null;
        }}
        public bool IsVisited {private set; get;}
        public bool IsPlayerHere {private set; get;}
        private bool isProtected;
        public bool IsSelected {private set; get;}
        public bool IsExposed {private set; get;}
        public bool IsProtected {set {
            this.isProtected = value;
            //if(isProtected)
            //this.Icon = 'P';
        } get {return this.isProtected;}}
        public Path(bool player = false) {
            this.IsPlayerHere = player;
            this.IsExposed = false;
            this.IsMined = false;
            if(player) {
                this.Icon = '0';
                this.IsVisited = true;
            } else {
                this.Icon = ' ';
                this.IsVisited = false;
            }
        }

        public Path(InventoryItem item) {
            this.IsPlayerHere = false;
            this.IsExposed = false;
            this.IsMined = false;
            this.IsVisited = false;
            this.Icon = item.Icon;
            this.item = item;
        }

        public void Expose() {
            this.IsExposed = true;
        }

        public InventoryItem PlayerEntered(char newIcon = '0') {
            this.IsPlayerHere = true;
            this.Icon = newIcon;
            this.IsVisited = true;
            if(this.ContainsItem) {
                InventoryItem i = this.item;
                this.item = null;
                return i;
            }
            return null;
        }
        public void PlayerLeft() {
            this.Icon = '.';
            this.IsPlayerHere = true;
        }
        public void PutItem(InventoryItem item) {
            this.item = item;
            this.Icon = item.Icon;
        }

        public void Select() {
            this.IsSelected = true;
        }
        
        public void Unselect() {
            this.IsSelected = false;
        }
    }
}