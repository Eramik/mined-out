namespace Mined_Out {
    public class Wall : Cell {
        public Wall() {
            this.Icon = '#';
        }

        override public void Select() {
            this.IsSelected = true;
        }
        
        override public void Unselect() {
            this.IsSelected = false;
        }
    }
}