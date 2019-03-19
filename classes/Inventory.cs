using System.Collections.Generic;

namespace Mined_Out {
    public class Inventory {
        private List<InventoryItem> items = new  List<InventoryItem>();
        public void Add(InventoryItem item) {
            items.Add(item);
        }
    }
}