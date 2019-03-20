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
        public string[] ToArrayOfStrings(int active = 0) {
            string[] s = new string[items.Count + 2];
            s[0] = "Inventory:";
            s[1] = "";
            int i = 2;
            foreach(InventoryRecord r in items) {
                s[i] = r.i.Type + " (" + r.amount + ")";
                if(i == active) {
                    s[i] += " <<<";
                }
                i++;
            }
            return s;
        }
        public void Activate(Field f) {
            if(items.Count == 0) return;
            int active = 2;
            while(true) {
                f.PrintToConsole(ToArrayOfStrings(active));
                var k = Console.ReadKey(true);
                var key = k.Key;
                if(key == ConsoleKey.S || key == ConsoleKey.DownArrow) {
                    if(active - 1 < items.Count) {
                        active++;
                    }
                } else if(key == ConsoleKey.W || key == ConsoleKey.UpArrow) {
                    if(active - 3 >= 0) {
                        active--;
                    }
                } else if(key == ConsoleKey.Spacebar || key == ConsoleKey.Enter) {
                    bool activated = items[active-2].i.Activate(f);
                    if(activated) {
                        if(items[active-2].amount == 1) {
                            items.RemoveAt(active-2);
                        } else {
                            items[active-2].amount--;
                        }
                    }
                    return;
                } else if(key == ConsoleKey.Escape) {
                    return;
                }
            }
        }
        public void Add(InventoryItem item) {
            int i = items.FindIndex((InventoryRecord r) => r.i.Type == item.Type);
            if(i != -1) {
                items[i].amount++;
                return;
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