namespace Mined_Out {
    public class InventoryItem {
        public char Icon {protected set; get;}
        public string Type {protected set; get;} 
        public int ChanceToSpawn {protected set; get;}
        public virtual bool Activate(Field f)
        {
            return false;
        }
    }
}