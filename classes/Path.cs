namespace Mined_Out {
    public class Path : Cell {
        public bool IsMined {set {
            isMined = value;
        } get {return isMined;}}
        private bool isMined;
        public bool Checked = false;
        private InventoryItem item = null;
        public bool ContainsItem {get {
            return item != null;
        }}
        public bool IsVisited {private set; get;}
        public bool IsPlayerHere {private set; get;}
        private bool isProtected;
        public int PlayerNumber {private set; get;}
        public bool IsExposed {private set; get;}
        public bool IsProtected {set {
            this.isProtected = value;
        } get {return this.isProtected;}}
        public Path(bool player = false, int playerNumber = 1) {
            this.IsPlayerHere = player;
            this.IsExposed = false;
            this.IsMined = false;
            if(player) {
                this.Icon = '0';
                this.IsVisited = true;
                this.PlayerNumber = playerNumber;
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
            if(!IsMined) {
                this.IsVisited = true;
                if(!IsPlayerHere) {
                    this.Icon = '.';
                }
            } else {
                this.Icon = '✘';
            }
        }
        public void Hide() {
            this.IsExposed = false;
            if(!IsPlayerHere && item == null) {
                this.Icon = ' ';
                this.IsVisited = false;
            }
            if(item != null) {
                this.IsVisited = false;
            }
        }

        public InventoryItem PlayerEntered(char newIcon = '0', int playerNumber = 1) {
            this.IsPlayerHere = true;
            this.Icon = newIcon;
            this.IsVisited = true;
            this.PlayerNumber = playerNumber;
            if(this.ContainsItem) {
                InventoryItem i = this.item;
                this.item = null;
                return i;
            }
            return null;
        }
        public void PlayerLeft() {
            this.Icon = '.';
            this.IsPlayerHere = false;
        }
        public void PutItem(InventoryItem item) {
            this.item = item;
            this.Icon = item.Icon;
        }

        override public void Select() {
            this.IsSelected = true;
        }
        
        override public void Unselect() {
            this.IsSelected = false;
        }
    }
}