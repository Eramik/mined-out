namespace Mined_Out {
    public class Wall : Path {
        public Wall() {
            this.Icon = '#';
            this.IsWall = true;
        }

        override public void Select() {
            this.IsSelected = true;
        }
        
        override public void Unselect() {
            this.IsSelected = false;
        }
    }
}