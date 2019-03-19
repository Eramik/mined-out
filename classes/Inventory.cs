using System.Collections.Generic;
using System;

namespace Mined_Out {
    public class Inventory {
        private class InventoryRecord {
            public InventoryItem i;
            public int amount;
            public InventoryRecord(InventoryItem i, int amount) {
                this.i = i;
                this.amount = amount;
            }
        }
        private List<InventoryRecord> items = new  List<InventoryRecord>();
        public void Add(InventoryItem item) {
            int i = items.FindIndex((InventoryRecord r) => r.i.Type == item.Type);
            if(i != -1) {
                items[i].amount++;
            }
            InventoryRecord newRecord = new InventoryRecord(item, 1);
            items.Add(newRecord);
        }

        public static Queue<InventoryItem> GenerateItemsToSpawn() {
            Random rnd = new Random();
            Queue<InventoryItem> q = new Queue<InventoryItem>();
            
            if(rnd.Next(100) < Scanner.ChanceToSpawn) {
                q.Enqueue(new Scanner());
            }

            return q;
        }
    }
}