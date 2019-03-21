namespace Mined_Out {
    public abstract class  Cell {
        public char Icon {protected set; get; }
        public bool IsSelected {protected set; get;}
        public abstract void Select();
        public abstract void Unselect();
    }
}